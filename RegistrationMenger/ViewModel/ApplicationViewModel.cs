using RegistrationMenger.Commands;
using RegistrationMenger.Models;
using RegistrationMenger.View;
using RegistrationMenger.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Windows;

namespace RegistrationMenger
{
    public class ApplicationViewModel : BusyViewModel
    {
        readonly ApplicationContext db;
        RelayCommand showNomeclatureWindowCommand;
        RelayCommand addCommand;
        RelayCommand clearCommand;
        IEnumerable<Acceptance> acceptance;
        IEnumerable<Nomenclature> nomenclature;
        IEnumerable<Shipment> shipment;

        private string acceptanceId;
        private string shipmentId;

        public IEnumerable<Acceptance> Acceptances
        {
            get { return acceptance; }
            set
            {
                acceptance = value;
                OnPropertyChanged("Acceptance");
            }
        }
        public IEnumerable<Shipment> Shipments
        {
            get { return shipment; }
            set
            {
                shipment = value;
                OnPropertyChanged("Shipment");
            }
        }
        public IEnumerable<Nomenclature> Nomenclatures
        {
            get { return nomenclature; }
            set
            {
                nomenclature = value;
                OnPropertyChanged("Nomenclature");
            }
        }

        public string AcceptanceId
        {
            get { return acceptanceId; }
            set
            {
                if (value == "")
                {
                    acceptanceId = null;
                    OnPropertyChanged("AcceptanceId");
                }
                else
                {
                    if (db.Nomenclatures.Find(value.ToLower()) != null)
                    {
                        acceptanceId = value.ToLower();
                        OnPropertyChanged("AcceptanceId");
                    }
                }
            }
        }
        public string ShipmentId
        {
            get { return shipmentId; }
            set
            {
                if (value == "")
                {
                    shipmentId = null;
                    OnPropertyChanged("ShipmentId");
                }
                else
                {
                    if (db.Nomenclatures.Find(value.ToLower()) != null)
                    {
                        shipmentId = value.ToLower();
                        OnPropertyChanged("ShipmentId");
                    }
                }

            }
        }

        public ApplicationViewModel()
        {
            db = new ApplicationContext();
            Thread.Sleep(4000);
            db.Acceptances.Load();
            db.Nomenclatures.Load();
            db.Shipments.Load();
            Acceptances = db.Acceptances.Local.ToBindingList();
            Shipments = db.Shipments.Local.ToBindingList();
            Nomenclatures = db.Nomenclatures.Local.ToBindingList();
        }

        public RelayCommand AddCommand => addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      try
                      {
                          OnBusy("Загрузка данных...");
                          Add();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show($"{ex.Message} " +
                              "Запустите приложение с правами администратора. Или переустановите приложение, " +
                              "не используя в пути системные директории (например устанновите программу в папку Пользователи)." +
                               "Пришлите мне письмо с описание выших действи вызваших эту ошибку. " +
                              @"bosup@bk.ru" + " Спасибо!",
                              "Регистратор - Ошибка!",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                      }
                      finally
                      {

                          OffBusy();
                      }
                  }));


        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                    (clearCommand = new RelayCommand((o) =>
                    {
                        OnBusy("Очистка данных...");
                        try
                        {
                            db.Acceptances.RemoveRange(Acceptances);
                            db.Shipments.RemoveRange(Shipments);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message} " +
                                "Запустите приложение с правами администратора. Или переустановите приложение, " +
                                "не используя в пути системные директории (например устанновите программу в папку Пользователи)." +
                                 "Пришлите мне письмо с описание выших действи вызваших эту ошибку. " +
                                @"bosup@bk.ru" + " Спасибо!",
                                "Регистратор - Ошибка!",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        finally
                        {
                            OffBusy();
                        }
                    }));
            }
        }


        public RelayCommand ShowNomclatureWindowCommand
        {
            get
            {
                return showNomeclatureWindowCommand ??
                    (showNomeclatureWindowCommand = new RelayCommand((o) =>
                      {
                          OnBusy("Получение данных для справочника номеклатуры...");
                          var model = new NomeclatureViewModel()
                          {
                              db = db,
                              Nomenclatures = Nomenclatures
                          };
                          var window = new NomeclatureWindow
                          {
                              DataContext = model,
                              Owner = Application.Current.MainWindow,
                              WindowStartupLocation = WindowStartupLocation.CenterOwner
                          }.ShowDialog();
                          OffBusy();
                      }));
            }
        }

        public void Add()
        {

            if (AcceptanceId != null || ShipmentId != null)
            {
                bool doubleSave = false;

                if (AcceptanceId != null && ShipmentId != null)
                {

                    if (AcceptanceId == ShipmentId)
                        doubleSave = true;
                    if (AcceptanceId.Contains(" "))
                    {
                        var acceptanceIds = AcceptanceId.Split();
                        foreach (var item in acceptanceIds)
                        {
                            SaveAcceptance(item, doubleSave);
                        }
                    }
                    else
                    {
                        SaveAcceptance(AcceptanceId, doubleSave);
                    }
                    if (ShipmentId.Contains(" "))
                    {
                        var shipmentIds = ShipmentId.Split();
                        foreach (var item in shipmentIds)
                        {
                            SaveShipment(item, doubleSave);
                        }
                    }
                    else
                    {
                        SaveShipment(ShipmentId, doubleSave);
                    }
                }
                else
                {
                    if (AcceptanceId != null)
                    {
                        if (AcceptanceId.Contains(" "))
                        {
                            var acceptanceIds = AcceptanceId.Split();
                            foreach (var item in acceptanceIds)
                            {
                                SaveAcceptance(item, doubleSave);
                            }
                        }
                        else
                        {
                            SaveAcceptance(AcceptanceId, doubleSave);
                        }
                    }
                    else
                    {
                        if (ShipmentId.Contains(" "))
                        {
                            var shipmentIds = ShipmentId.Split();
                            foreach (var item in shipmentIds)
                            {
                                SaveShipment(item, doubleSave);
                            }
                        }
                        else
                        {
                            SaveShipment(ShipmentId, doubleSave);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        public void SaveAcceptance(string acceptanceId, bool doubleSave)
        {
            Acceptance item = new Acceptance();

            if (db.Nomenclatures.Find(acceptanceId) == null)
                return;

            if (db.Acceptances.Find(AcceptanceId) == null)
            {
                item.Id = acceptanceId;
                item.Name = db.Nomenclatures.Find(acceptanceId).Name;
                item.Qte = 1;
                db.Acceptances.Add(item);
                //db.SaveChanges();
            }
            else
            {
                item = db.Acceptances.Find(AcceptanceId);
                item.Qte += 1;
                db.Entry(item).State = EntityState.Modified;
                //db.SaveChanges();
            }

            if (doubleSave)
                return;
            if (db.Shipments.Find(acceptanceId) != null)
            {

                Shipment shipmentItem = db.Shipments.Find(acceptanceId);

                if (shipmentItem.Qte == 1)
                {
                    db.Shipments.Remove(shipmentItem);
                    //db.SaveChanges();
                }
                else
                {
                    shipmentItem.Qte -= 1;
                    db.Entry(shipmentItem).State = EntityState.Modified;
                    //db.SaveChanges();
                }
            }
        }

        public void SaveShipment(string shipmentId, bool doubleSave)
        {
            Shipment item = new Shipment();

            if (db.Nomenclatures.Find(shipmentId) == null)
                return;

            if (db.Shipments.Find(ShipmentId) == null)
            {
                item.Id = shipmentId;
                item.Name = db.Nomenclatures.Find(shipmentId).Name;
                item.Qte = 1;
                db.Shipments.Add(item);
                //db.SaveChanges();
            }
            else
            {
                item = db.Shipments.Find(ShipmentId);
                item.Qte += 1;
                db.Entry(item).State = EntityState.Modified;
                //db.SaveChanges();
            }

            if (doubleSave)
                return;

            if (db.Acceptances.Find(shipmentId) != null)
            {
                Acceptance acceptanceItem = db.Acceptances.Find(shipmentId);

                if (acceptanceItem.Qte == 1)
                {
                    db.Acceptances.Remove(acceptanceItem);
                    //db.SaveChanges();
                }
                else
                {
                    acceptanceItem.Qte -= 1;
                    db.Entry(acceptanceItem).State = EntityState.Modified;
                    //db.SaveChanges();
                }
            }
        }
    }
}
