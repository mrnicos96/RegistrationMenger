using RegistrationMenger.Commands;
using RegistrationMenger.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RegistrationMenger.ViewModel
{
    public class NomeclatureViewModel : BusyViewModel
    {
        public ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand clearCommand;
        RelayCommand deleteCommand;
        IEnumerable<Nomenclature> nomenclature;
        private string id;
        private string name;
        public int b;
        public IEnumerable<Nomenclature> Nomenclatures
        {
            get { return nomenclature; }
            set
            {
                nomenclature = value;
                OnPropertyChanged("Nomeclature");
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value.ToLower();
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public NomeclatureViewModel() { }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      OnBusy("Загрузка данных...");
                      try
                      {
                          if (Id != null && Name != null && db.Nomenclatures.Find(Id) == null)
                          {
                              Nomenclature item = new Nomenclature();
                              item.Id = Id;
                              item.Name = Name;
                              db.Nomenclatures.Add(item);
                              db.SaveChanges();
                          }
                          Id = "";
                          Name = "";
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

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      OnBusy("Очистка данных...");
                      Nomenclature item = selectedItem as Nomenclature;
                      db.Nomenclatures.Remove(item);
                      db.SaveChanges();
                      OffBusy();
                  }));
            }
        }

        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                    (clearCommand = new RelayCommand((o) =>
                     {
                         try
                         {
                             OnBusy("Очистка данных...");
                             db.Nomenclatures.RemoveRange(Nomenclatures);
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
    }
}
