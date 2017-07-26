using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ping_Monitor
{
    public class PingerViewModel : INotifyPropertyChanged
    {
        IFileService fileService;
        IDialogService dialogService;
        Window mainWindow;

        public ObservableCollection<Pinger> Pingers { get; set; }
        public Settings Settings { get; set; }

        public PingerViewModel(IDialogService dialogService, IFileService fileService, Window mainWindow)
        {
            this.mainWindow = mainWindow;
            this.dialogService = dialogService;
            this.fileService = fileService;
            Pingers = new ObservableCollection<Pinger>();
            Settings = new Settings();

            FileInfo fileInfo = new FileInfo("Ping Monitor.lst");
            if (fileInfo.Exists)
            {
                var pingers = fileService.OpenLST("Ping Monitor.lst");
                foreach (var p in pingers)
                    Pingers.Add(p);
            }
            if (Pingers.Count==0)
            {
                // данные по умлолчанию
                Pingers.Add(new Pinger {Name="Яndex", Address="yandex.ru", TTL=128, Fragment=false, Log=false});
                Pingers.Add(new Pinger {Name="Google DNS", Address="8.8.8.8", TTL=128, Fragment=false, Log=false});
                Pingers.Add(new Pinger {Name="Localhost", Address="127.0.0.1", TTL=128, Fragment=false, Log=false});
                Pingers.Add(new Pinger {Name="Local", Address="192.168.1.1", TTL=128, Fragment=false, Log=false});
                try
                {
                    fileService.SaveLST("Ping Monitor.lst", Pingers.ToList());
                }
                catch
                {
                    MessageBox.Show(fileService.Save_File_Error);
                }
            }

            fileInfo = new FileInfo("Ping Monitor.cfg");
            if (fileInfo.Exists)
            {
                Settings = fileService.OpenCFG("Ping Monitor.cfg");
            }
            if (Settings.Timeout == 0)
            {
                // данные по умлолчанию
                Settings.Timeout = 4000;
                Settings.Pause = 2000;
                Settings.Allert_Enable = false;
                Settings.Allert_Errors = 5;
                Settings.Email_Enable = false;
                Settings.Email_Errors = 10;
                Settings.Email_Pause = 30;

                try
                {
                    fileService.SaveCFG("Ping Monitor.cfg", Settings);
                }
                catch
                {
                    MessageBox.Show(fileService.Save_File_Error);
                }
            }
        }
        

        Pinger selectedPinger;
        public Pinger SelectedPinger
        {
            get { return selectedPinger; }
            set
            {
                selectedPinger = value;
                OnPropertyChanged("SelectedPinger");
            }
        }

        private RelayCommand settings_Command;
        public RelayCommand Settings_Command
        {
            get
            {
                return settings_Command ??
                (settings_Command = new RelayCommand(obj =>
                {
                    SettingsWindow settingsWindow = new SettingsWindow(Settings);
                    settingsWindow.Owner = mainWindow;
                    if (settingsWindow.ShowDialog() == false)
                    {
                        //При закрытии окна настроек
                        if (!String.IsNullOrEmpty(settingsWindow.New_Pass.Password))
                        {
                            Settings.Email_Pass =  Encrypter.Encrypt(settingsWindow.New_Pass.Password);
                        }
                        try
                        {
                            fileService.SaveCFG("Ping Monitor.cfg", Settings);
                        }
                        catch
                        {
                            MessageBox.Show(fileService.Save_File_Error);
                        }
                    }
                }));
            }
        }

        private RelayCommand save_Command;
        public RelayCommand Save_Command
        {
            get
            {
                return save_Command ??
                (save_Command = new RelayCommand(obj =>
                {
                    try
                    {
                        if (dialogService.SaveFileDialog() == true)
                        {
                            fileService.SaveLST(dialogService.FilePath, Pingers.ToList());
                        }
                    }
                    catch (Exception ex)
                    {
                        dialogService.ShowMessage(ex.Message);
                    }
                }));
            }
        }

        private RelayCommand open_Command;
        public RelayCommand Open_Command
        {
            get
            {
                return open_Command ??
                (open_Command = new RelayCommand(obj =>
                {
                    try
                    {
                        if (dialogService.OpenFileDialog() == true)
                        {
                            var pingers = fileService.OpenLST(dialogService.FilePath);
                            if (pingers.Count > 0)
                            {
                                Pingers.Clear();
                                foreach (var p in pingers)
                                    Pingers.Add(p);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show(fileService.Open_File_Error);
                    }
                }));
            }
        }

        private RelayCommand add_Command;
        public RelayCommand Add_Command
        {
            get
            {
                return add_Command ??
                (add_Command = new RelayCommand(obj =>
                {
                    Pinger pinger = new Pinger { Address = "0.0.0.0", TTL = 128, Fragment = false, Log = false};
                    Pingers.Insert(0, pinger);
                    SelectedPinger = pinger;
                    Edit(pinger, mainWindow, fileService, Pingers);
                }));
            }
        }

        private RelayCommand remove_Command;
        public RelayCommand Remove_Command
        {
            get
            {
                return remove_Command ??
                (remove_Command = new RelayCommand(obj =>
                {
                    Pinger pinger = obj as Pinger;
                    if (pinger != null)
                    {
                        Pingers.Remove(pinger);
                        try
                        {
                            fileService.SaveLST("Ping Monitor.lst", Pingers.ToList());
                        }
                        catch
                        {
                            MessageBox.Show("Error");
                        }
                    }
                },
                (obj) => (Pingers.Count > 0 && SelectedPinger != null && SelectedPinger.In_Ping == false)));
            }
        }

        private RelayCommand ping_Command;
        public RelayCommand Ping_Command
        {
            get
            {
                return ping_Command ??
                (ping_Command = new RelayCommand(obj =>
                {
                    Pinger pinger = obj as Pinger;
                    if (pinger != null)
                    {
                        pinger.In_Ping = true;
                        Task task = new Task(() => Ping(pinger, Settings));
                        task.Start();
                    }

                },
                (obj) => (SelectedPinger != null && SelectedPinger.In_Ping == false)));
            }
        }

        private RelayCommand stop_Command;
        public RelayCommand Stop_Command
        {
            get
            {
                return stop_Command ??
                (stop_Command = new RelayCommand(obj =>
                {
                    Pinger pinger = obj as Pinger;
                    if (pinger != null)
                    {
                        pinger.In_Ping = false;
                    }

                },
                (obj) => (SelectedPinger != null && SelectedPinger.In_Ping == true)));
            }
        }

        private RelayCommand ping_all_Command;
        public RelayCommand Ping_All_Command
        {
            get
            {
                return ping_all_Command ??
                (ping_all_Command = new RelayCommand(obj =>
                {
                    foreach (Pinger pinger in Pingers)
                    {
                        if (pinger.In_Ping == false)
                        {
                            pinger.In_Ping = true;
                            Task task = new Task(() => Ping(pinger, Settings));
                            task.Start();
                        }
                    }

                },
                (obj) => (Pingers.Count > 0)));
            }
        }

        private RelayCommand stop_all_Command;
        public RelayCommand Stop_All_Command
        {
            get
            {
                return stop_all_Command ??
                (stop_all_Command = new RelayCommand(obj =>
                {
                    foreach (Pinger pinger in Pingers)
                    {
                        pinger.In_Ping = false;
                    }

                },
                (obj) => (Pingers.Count > 0)));
            }
        }

        private RelayCommand edit_Command;
        public RelayCommand Edit_Command
        {
            get
            {
                return edit_Command ??
                (edit_Command = new RelayCommand(obj =>
                {
                    Pinger pinger = obj as Pinger;
                    if (pinger != null)
                    {
                        Edit(pinger, mainWindow, fileService, Pingers);
                    }

                },
                (obj) => (SelectedPinger != null && SelectedPinger.In_Ping == false)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        static void Edit(Pinger pinger, Window mainWindow, IFileService fileService, ObservableCollection<Pinger> Pingers)
        {
            EditWindow editWindow = new EditWindow(pinger);
            editWindow.Owner = mainWindow;
            if (editWindow.ShowDialog() == false)
            {
                //При закрытии окна редактирования
                try
                {
                        fileService.SaveLST("Ping Monitor.lst", Pingers.ToList());
                }
                catch
                {
                    MessageBox.Show(fileService.Save_File_Error);
                }
            }
        }

        static void Ping(Pinger pinger, Settings settings)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            //Options
            options.Ttl = pinger.TTL != 0 ? pinger.TTL : 128;
            options.DontFragment = pinger.Fragment;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            PingReply reply = null;

            //Log usage
            StreamWriter sw = null;
            if (pinger.Log == true)
            {
                try //Try to create filestream
                {
                    string file_name = $"{pinger.Address.Replace(".", "_")}.csv";
                    sw = new StreamWriter(file_name, false, System.Text.Encoding.Default);
                    sw.AutoFlush = true;
                    sw.WriteLine("Time,Status,Roundtrip Time,TTL"); //Header sting
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            do
            {
                try
                {
                    pinger.Total++;
                    reply = pingSender.Send(pinger.Address, settings.Timeout, buffer, options);
                }
                catch
                {
                    pinger.Status = reply != null ? reply.Status.ToString() : "Error";
                }
                finally
                {
                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        pinger.Status = reply.Status.ToString();
                        pinger.Time_Total += reply.RoundtripTime;
                        if (reply.RoundtripTime > pinger.Time_Max) pinger.Time_Max = reply.RoundtripTime;
                        if ((reply.RoundtripTime < pinger.Time_Min && reply.RoundtripTime!=0) || pinger.Time_Min==0) pinger.Time_Min = reply.RoundtripTime;
                        pinger.Time_Cur = reply.RoundtripTime;
                        pinger.TTL_Return = reply.Options.Ttl;
                        pinger.Error_Count=0;
                    }
                    else
                    {
                        pinger.Status = reply != null ? reply.Status.ToString() : "Error";
                        pinger.Error_Count++;
                        pinger.Lost_Count++;
                    }
                    if (pinger.Log == true)
                    {
                        sw.WriteLine($"{DateTime.Now.ToString()},{pinger.Status},{pinger.Time_Cur},{pinger.TTL_Return}");
                    }
                    if (pinger.Error_Count!=0 && settings.Allert_Enable == true && pinger.Error_Count % settings.Allert_Errors == 0)
                    {
                        MessageBox.Show($"{DateTime.Now.ToString()} - connection lost on {pinger.Name} ({pinger.Address})!", $"Ping Monitor Error ({pinger.Name})");
                    }
                    if (pinger.Error_Count != 0 && settings.Email_Enable == true
                        && pinger.Error_Count % settings.Email_Errors == 0
                        && pinger.Email_Time.AddMinutes(settings.Email_Pause) < DateTime.Now)
                    {
                        SmtpClient client = new SmtpClient(settings.Email_Server, settings.Email_Port);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential(settings.Email_Login, Encrypter.Decrypt(settings.Email_Pass));
                        client.EnableSsl = settings.Email_Secure;
                        MailMessage mail = new MailMessage(settings.Email_Login, settings.Email_Address);
                        mail.Subject = $"Ping Monitor Error ({pinger.Name})";
                        mail.Body = $"{DateTime.Now.ToString()} - connection lost on {pinger.Name} ({pinger.Address})!";
                        mail.IsBodyHtml = false;
                        client.SendMailAsync(mail);
                        pinger.Email_Time = DateTime.Now;
                    }
                }
                pinger.Time_Avg = pinger.Time_Total / pinger.Total;
                pinger.Lost = pinger.Lost_Count*100/pinger.Total;
                Thread.Sleep(settings.Pause); //пауза между пингами
            }
            while (pinger.In_Ping != false);

            if (pinger.Log == true) sw.Close(); //Закрываем StreamWriter при остановке
        }
    }
}
