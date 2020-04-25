using DataTool.Models.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTool
{
    public partial class MainForm : Form
    {
        public class MultiList<T, T2> : List<T>
        {
            public List<T2> InnerData = new List<T2>();
        }
        public MainForm() => InitializeComponent();
        private void MainForm_Load(object sender, EventArgs e) => PopulateTreeView();
        private TreeNode[] lastSelectedNodes = new TreeNode[] { };
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

        private void SelectGame(params DataGame[] game)
        {
            listBoxTgdbIds.BeginUpdate();
            listBoxTgdbIds.DataSource = null;
            listBoxTgdbIds.Items.Clear();

            if (game.Length == 1)
            {
                listBoxTgdbIds.DataSource = game[0].TgdbId;
            }
            else if (game.Length > 1)
            {
                var lists = game.Select(l => l.TgdbId);
                var intersection = lists
                    .Skip(1)
                    .Aggregate(
                        new HashSet<int>(lists.First()),
                        (h, e) => { h.IntersectWith(e); return h; }
                    );
                var list = new MultiList<int, DataGame>();
                list.InnerData.AddRange(game);
                list.AddRange(intersection.ToList());
                listBoxTgdbIds.DataSource = list;
            }

            listBoxTgdbIds.EndUpdate();
            listBoxTgdbIds.EndUpdate();
        }

        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selected = treeViewMain.SelectedNodes;
            var selectedGames = selected.Where(g => g.Tag is DataGame).ToList();

            groupBoxGameInformation.Enabled = false;
            buttonDeleteDataSet.Enabled = false;
            buttonImportDataSet.Enabled = true;
            labelNameData.Text = null;

            listBoxTgdbIds.BeginUpdate();
            listBoxTgdbIds.DataSource = null;
            listBoxTgdbIds.Items.Clear();
            listBoxTgdbIds.EndUpdate();

            if (selected.Count == 0)
            {
                return;
            }

            if (selected.Count == 1 && selected[0].Tag is DataGroup)
            {
                var group = (DataGroup)selected[0].Tag;
                buttonDeleteDataSet.Enabled = true;
            }
            if (selectedGames.Count > 0)
            {
                groupBoxGameInformation.Enabled = true;
                SelectGame(selectedGames.Select(g => g.Tag as DataGame).ToArray());
            }

            if (selectedGames.Count == 1)
            {
                var game = selected[0].Tag as DataGame;
                labelNameData.Text = game.Name;
                
            }

            buttonMoveIdUp.Enabled = selectedGames.Count == 1;
            buttonMoveIdDown.Enabled = selectedGames.Count == 1;

            treeViewMain.BeginUpdate();
            foreach (TreeNode node in lastSelectedNodes)
            {
                UpdateNodeColor(node);
            }

            foreach (TreeNode node in treeViewMain.SelectedNodes)
            {
                node.ForeColor = SystemColors.HighlightText;
            }

            treeViewMain.EndUpdate();
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

                        if (listBoxTgdbIds.DataSource is MultiList<int, DataGame>)
                        {
                            foreach (var game in (listBoxTgdbIds.DataSource as MultiList<int, DataGame>).InnerData)
                            {
                                if (!game.TgdbId.Contains(numberEntry.Number))
                                {
                                    game.TgdbId.Add(numberEntry.Number);
                                }
                            }
                        }

                        listBoxTgdbIds.DataSource = null;
                        listBoxTgdbIds.DataSource = list;
                        listBoxTgdbIds.SelectedIndex = list.Count - 1;
                    }

                    listBoxTgdbIds.EndUpdate();
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

            if (listBoxTgdbIds.DataSource is MultiList<int, DataGame>)
            {
                foreach (var game in (listBoxTgdbIds.DataSource as MultiList<int, DataGame>).InnerData)
                {
                    game.TgdbId.RemoveAll(i => i == (int)listBoxTgdbIds.SelectedItem);
                }
            }

            listBoxTgdbIds.DataSource = null;
            listBoxTgdbIds.DataSource = list;
            listBoxTgdbIds.EndUpdate();
        }

        private void UpdateNodeColor(params TreeNode[] nodes)
        {
            foreach (var node in nodes.Where(e => e.Tag is DataGame))
            {
                var game = node.Tag as DataGame;

                node.ForeColor = (node.IsSelected ? SystemColors.HighlightText : (game.TgdbId.Count > 0 ? Color.Green : Color.Red));
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

        private void buttonImportDataSet_Click(object sender, EventArgs e)
        {
            var result = ProgressForm.Start((taskHost) =>
            {
                taskHost.SetTitle("Importing Data");
                taskHost.SetStatus("Selecting file");
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = ".dat Files|*.dat";
                    ofd.Multiselect = true;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var filename in ofd.FileNames)
                        {
                            if (File.Exists(filename))
                            {
                                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    taskHost.SetStatus("Deserializing file");

                                    var data = Xml.Deserialize<DataGroup>(file);
                                    long count = 0;
                                    var gameCount = data.Games.Count;


                                    foreach (var newGame in data.Games)
                                    {
                                        var hashSequence = newGame.Roms.Select(h => h.Crc32.ToLower()).OrderBy(h => h);

                                        taskHost.SetStatus($"Processing: {newGame.Name}");
                                        foreach (var group in Program.Collection.ToArray())
                                        {
                                            Parallel.ForEach(group.Games.ToArray(), game =>
                                            {
                                                if (hashSequence.SequenceEqual(game.Roms.Select(h => h.Crc32.ToLower()).OrderBy(h => h)))
                                                {
                                                    newGame.TgdbId.AddRange(game.TgdbId.Except(newGame.TgdbId));
                                                    group.Games.Remove(game);
                                                }
                                            });

                                            if (group.Games.Count == 0)
                                            {
                                                Program.Collection.Remove(group.Delete());
                                            }
                                        }

                                        taskHost.SetProgress(++count, gameCount);
                                    }

                                    if (data.Games.Count > 0)
                                    {
                                        Program.Collection.Add(data);
                                    }
                                }
                            }
                        }
                    }
                }

                return ProgressForm.Result.Success;
            });

            PopulateTreeView();
        }

        private void treeViewMain_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            lastSelectedNodes = treeViewMain.SelectedNodes.ToArray();
        }
    }
}
