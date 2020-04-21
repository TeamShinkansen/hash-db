namespace DataTool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewMain = new System.Windows.Forms.TreeView();
            this.groupBoxGameInformation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelNameData = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxTgdbIds = new System.Windows.Forms.ListBox();
            this.buttonMoveIdUp = new System.Windows.Forms.Button();
            this.buttonMoveIdDown = new System.Windows.Forms.Button();
            this.buttonAddId = new System.Windows.Forms.Button();
            this.buttonRemoveId = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDeleteDataSet = new System.Windows.Forms.Button();
            this.buttonImportDataSet = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxGameInformation.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.treeViewMain, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxGameInformation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // treeViewMain
            // 
            resources.ApplyResources(this.treeViewMain, "treeViewMain");
            this.treeViewMain.Name = "treeViewMain";
            this.tableLayoutPanel1.SetRowSpan(this.treeViewMain, 2);
            this.treeViewMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMain_AfterSelect);
            // 
            // groupBoxGameInformation
            // 
            this.groupBoxGameInformation.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.groupBoxGameInformation, "groupBoxGameInformation");
            this.groupBoxGameInformation.Name = "groupBoxGameInformation";
            this.groupBoxGameInformation.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelNameData, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // labelName
            // 
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Name = "labelName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labelNameData
            // 
            resources.ApplyResources(this.labelNameData, "labelNameData");
            this.labelNameData.Name = "labelNameData";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.buttonMoveIdUp, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonMoveIdDown, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonAddId, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.listBoxTgdbIds, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonRemoveId, 1, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // listBoxTgdbIds
            // 
            resources.ApplyResources(this.listBoxTgdbIds, "listBoxTgdbIds");
            this.listBoxTgdbIds.FormattingEnabled = true;
            this.listBoxTgdbIds.Name = "listBoxTgdbIds";
            this.tableLayoutPanel3.SetRowSpan(this.listBoxTgdbIds, 4);
            // 
            // buttonMoveIdUp
            // 
            resources.ApplyResources(this.buttonMoveIdUp, "buttonMoveIdUp");
            this.buttonMoveIdUp.Name = "buttonMoveIdUp";
            this.buttonMoveIdUp.UseVisualStyleBackColor = true;
            this.buttonMoveIdUp.Click += new System.EventHandler(this.buttonMoveIdUp_Click);
            // 
            // buttonMoveIdDown
            // 
            resources.ApplyResources(this.buttonMoveIdDown, "buttonMoveIdDown");
            this.buttonMoveIdDown.Name = "buttonMoveIdDown";
            this.buttonMoveIdDown.UseVisualStyleBackColor = true;
            this.buttonMoveIdDown.Click += new System.EventHandler(this.buttonMoveIdDown_Click);
            // 
            // buttonAddId
            // 
            resources.ApplyResources(this.buttonAddId, "buttonAddId");
            this.buttonAddId.Name = "buttonAddId";
            this.buttonAddId.UseVisualStyleBackColor = true;
            this.buttonAddId.Click += new System.EventHandler(this.buttonAddId_Click);
            // 
            // buttonRemoveId
            // 
            resources.ApplyResources(this.buttonRemoveId, "buttonRemoveId");
            this.buttonRemoveId.Name = "buttonRemoveId";
            this.buttonRemoveId.UseVisualStyleBackColor = true;
            this.buttonRemoveId.Click += new System.EventHandler(this.buttonRemoveId_Click);
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.buttonDeleteDataSet, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonImportDataSet, 1, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // buttonDeleteDataSet
            // 
            resources.ApplyResources(this.buttonDeleteDataSet, "buttonDeleteDataSet");
            this.buttonDeleteDataSet.ForeColor = System.Drawing.Color.Red;
            this.buttonDeleteDataSet.Name = "buttonDeleteDataSet";
            this.buttonDeleteDataSet.UseVisualStyleBackColor = true;
            this.buttonDeleteDataSet.Click += new System.EventHandler(this.buttonDeleteDataSet_Click);
            // 
            // buttonImportDataSet
            // 
            resources.ApplyResources(this.buttonImportDataSet, "buttonImportDataSet");
            this.buttonImportDataSet.Name = "buttonImportDataSet";
            this.buttonImportDataSet.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxGameInformation.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView treeViewMain;
        private System.Windows.Forms.GroupBox groupBoxGameInformation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelNameData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonMoveIdUp;
        private System.Windows.Forms.Button buttonMoveIdDown;
        private System.Windows.Forms.Button buttonAddId;
        private System.Windows.Forms.ListBox listBoxTgdbIds;
        private System.Windows.Forms.Button buttonRemoveId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button buttonDeleteDataSet;
        private System.Windows.Forms.Button buttonImportDataSet;
    }
}