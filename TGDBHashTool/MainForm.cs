using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGDBHashTool.Models;
using TGDBHashTool.Models.Data;

namespace TGDBHashTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //new Thread(() => LovePack()).Start();
            PopulateTreeView();
        }
        private void LovePack()
        {
            var datFiles = new DataCollection();
            var allDatFiles = new DataCollection();
            UInt64 counter = 0;

            using (TextReader reader = new StreamReader("../../hash.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.RegisterClassMap<HashCsv.Map>();
                var records = csv.GetRecords<HashCsv>().Where(e => e.TgdbId != null).ToArray();
                var matchedRecords = new List<HashCsv>();

                int filesProcessed = 0;
                Parallel.ForEach(Directory.GetFiles("../../lovepack"), filename =>
                {
                    using (var file = File.OpenRead(filename))
                    {
                        var dat = Xml.Deserialize<DataGroup>(file);
                        var matched = false;
                        Parallel.ForEach(records, record =>
                        {
                            foreach (var game in dat.Games.Where(e => e.Roms.Where(r => r.Sha1 != null && r.Sha1.ToLower() == record.Sha1.ToLower()).Count() > 0))
                            {
                                if (!matchedRecords.Contains(record))
                                {
                                    matchedRecords.Add(record);
                                }
                                matched = true;
                                game.TgdbId.Add((int)record.TgdbId);

                            }
                        });

                        allDatFiles.Groups.Add(dat);

                        if (matched)
                        {
                            Program.Collection.Add(dat);
                            datFiles.Groups.Add(dat);
                        }
                    }
                    filesProcessed++;

                    Console.WriteLine($"Processed: {filename}");
                    Console.WriteLine($"Files Processed: {filesProcessed}");
                    Console.WriteLine("");
                });

                var unmatchedRecords = records.Except(matchedRecords).ToList();

                using (var file = File.Create("unmatched.xml")) {
                    Xml.Serialize<List<HashCsv>>(file, unmatchedRecords);
                }
            }
            Invoke(new Action(() => PopulateTreeView()));
        }

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
