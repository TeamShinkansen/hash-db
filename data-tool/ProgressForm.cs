using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DataTool
{
    public partial class ProgressForm : Form
    {
        public enum Result
        {
            Unknown,
            Success,
            Failure,
            Abort
        }
        public delegate Result Task(ProgressForm taskHost);
        private bool taskFinished = false;
        private bool isOpen = false;

        private string _Title;
        private string _Status;
        private int _Progress;
        private int _ProgressMax = int.MaxValue;

        public ProgressForm SetTitle(string title)
        {
            if (InvokeRequired)
            {
                return (ProgressForm)Invoke(new Func<ProgressForm>(() => SetTitle(title)));
            }

            _Title = title;
            if (!isOpen) return this;
            
            Text = title;

            return this;
        }

        public ProgressForm SetStatus(string status)
        {
            if (InvokeRequired)
            {
                return (ProgressForm)Invoke(new Func<ProgressForm>(() => SetStatus(status)));
            }

            _Status = status;
            if (!isOpen) return this;

            labelStatus.Text = status;

            return this;
        }

        public ProgressForm SetProgress(long current, long max)
        {
            if (InvokeRequired)
            {
                return (ProgressForm)Invoke(new Func<ProgressForm>(() => SetProgress(current, max)));
            }

            var progress = Math.Min(1.0 * current / max, 1.0);
            _Progress = Convert.ToInt32(Math.Truncate(progressBarMain.Maximum * progress));

            if (!isOpen) return this;

            progressBarMain.Value = _Progress;

            return this;
        }
        public ProgressForm()
        {
            InitializeComponent();
        }

        public static Result Start(Task taskHost)
        {
            var result = Result.Unknown;

            using (var form = new ProgressForm())
            {
                var taskThread = new Thread(() =>
                {
                    result = taskHost(form);
                    form.taskFinished = true;

                    try
                    {
                        form.Invoke(new Action(() =>
                        {
                            form.Close();
                        }));
                    } 
                    catch (Exception) { }
                });

                taskThread.SetApartmentState(ApartmentState.STA);

                form.FormClosed += (object sender, FormClosedEventArgs e) =>
                {
                    if (form.taskFinished == false)
                    {
                        taskThread.Abort();
                    }
                };

                taskThread.Start();
                form.ShowDialog();
            }

            return result;
        }

        private void ProgressForm_Shown(object sender, EventArgs e)
        {
            progressBarMain.Maximum = _ProgressMax;
            progressBarMain.Value = _Progress;
            Text = _Title;
            SetStatus(_Status);
            isOpen = true;

            if (taskFinished)
            {
                Close();
            }
        }
    }
}
