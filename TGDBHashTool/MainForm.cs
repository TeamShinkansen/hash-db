using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TGDBHashTool.Models.Data;

namespace TGDBHashTool
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        private void MainForm_Load(object sender, EventArgs e) => PopulateTreeView();

        private void PopulateTreeView()
        {

            treeViewMain.Nodes.Clear();
            
            foreach (var group in Program.Collection.OrderBy(e => e.Name))
            {
                var groupNode = new TreeNode()
                {
                    Text = group.Name,
                    Tag = group
                };

                foreach (var game in group.Games.OrderBy(e => e.Name))
                {
                    var gameNode = new TreeNode()
                    {
                        Text = game.Name,
                        Tag = game
                    };

                    UpdateNodeColor(gameNode);

                    groupNode.Nodes.Add(gameNode);
                }

                treeViewMain.Nodes.Add(groupNode);
            }

            treeViewMain.EndUpdate();
        }

        private void SelectGame(DataGame game)
        {
            listBoxTgdbIds.BeginUpdate();
            listBoxTgdbIds.DataSource = null;
            listBoxTgdbIds.Items.Clear();
            listBoxTgdbIds.DataSource = game.TgdbId;
            listBoxTgdbIds.EndUpdate();
            listBoxTgdbIds.EndUpdate();
        }

        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selected = treeViewMain.SelectedNode;

            groupBoxGameInformation.Enabled = false;
            buttonDeleteDataSet.Enabled = false;
            buttonImportDataSet.Enabled = true;
            labelNameData.Text = null;

            listBoxTgdbIds.BeginUpdate();
            listBoxTgdbIds.DataSource = null;
            listBoxTgdbIds.Items.Clear();
            listBoxTgdbIds.EndUpdate();

            if (selected == null)
            {
                return;
            }

            if (selected.Tag is DataGroup)
            {
                var group = (DataGroup)selected.Tag;
                buttonDeleteDataSet.Enabled = true;
            }

            if (selected.Tag is DataGame)
            {
                var game = (DataGame)selected.Tag;
                groupBoxGameInformation.Enabled = true;
                labelNameData.Text = game.Name;
                SelectGame(game);
            }
        }

        private void buttonMoveIdUp_Click(object sender, EventArgs e)
        {
            listBoxTgdbIds.BeginUpdate();
            if (listBoxTgdbIds.SelectedItems.Count == 0)
            {
                return;
            }

            var list = listBoxTgdbIds.DataSource as List<int>;

            var selected = (int)listBoxTgdbIds.SelectedItem;
            var selectedIndex = listBoxTgdbIds.SelectedIndex;

            if (selectedIndex > 0)
            {
                list.Remove(selected);
                list.Insert(selectedIndex - 1, selected);
                listBoxTgdbIds.DataSource = null;
                listBoxTgdbIds.DataSource = list;
                listBoxTgdbIds.SelectedIndex = selectedIndex - 1;
            }

            listBoxTgdbIds.EndUpdate();

            UpdateNodeColor(treeViewMain.SelectedNode);
        }

        private void buttonMoveIdDown_Click(object sender, EventArgs e)
        {
            listBoxTgdbIds.BeginUpdate();
            if (listBoxTgdbIds.SelectedItems.Count == 0)
            {
                return;
            }

            var list = listBoxTgdbIds.DataSource as List<int>;

            var selected = (int)listBoxTgdbIds.SelectedItem;
            var selectedIndex = listBoxTgdbIds.SelectedIndex;

            if (selectedIndex < (list.Count - 1))
            {
                list.Remove(selected);
                list.Insert(selectedIndex + 1, selected);
                listBoxTgdbIds.DataSource = null;
                listBoxTgdbIds.DataSource = list;
                listBoxTgdbIds.SelectedIndex = selectedIndex + 1;
            }

            listBoxTgdbIds.EndUpdate();

            UpdateNodeColor(treeViewMain.SelectedNode);
        }

        private void buttonAddId_Click(object sender, EventArgs e)
        {
            using (var numberEntry = new NumberEntry())
            {
                if (numberEntry.ShowDialog() == DialogResult.OK)
                {
                    listBoxTgdbIds.BeginUpdate();

                    var list = listBoxTgdbIds.DataSource as List<int>;

                    if (!list.Contains(numberEntry.Number))
                    {
                        list.Add(numberEntry.Number);
                        listBoxTgdbIds.DataSource = null;
                        listBoxTgdbIds.DataSource = list;
                        listBoxTgdbIds.SelectedIndex = list.Count - 1;
                    }

                    listBoxTgdbIds.EndUpdate();

                    UpdateNodeColor(treeViewMain.SelectedNode);
                }
            }
        }

        private void buttonRemoveId_Click(object sender, EventArgs e)
        {
            var selected = listBoxTgdbIds.SelectedItem;

            if (selected == null)
            {
                return;
            }

            var list = listBoxTgdbIds.DataSource as List<int>;

            listBoxTgdbIds.BeginUpdate();
            list.RemoveAt(listBoxTgdbIds.SelectedIndex);
            listBoxTgdbIds.DataSource = null;
            listBoxTgdbIds.DataSource = list;
            listBoxTgdbIds.EndUpdate();

            UpdateNodeColor(treeViewMain.SelectedNode);
        }

        private void UpdateNodeColor(TreeNode node)
        {
            if (node.Tag is DataGame)
            {
                var game = (DataGame)node.Tag;

                node.ForeColor = (game.TgdbId.Count > 0 ? Color.Green : Color.Red);
            }
        }

        private void buttonDeleteDataSet_Click(object sender, EventArgs e)
        {
            var selected = treeViewMain.SelectedNode;

            if (selected != null && selected.Tag is DataGroup)
            {
                var group = (DataGroup)selected.Tag;

                Program.Collection.Remove(group.Delete());

                PopulateTreeView();
            }
        }
    }
}
