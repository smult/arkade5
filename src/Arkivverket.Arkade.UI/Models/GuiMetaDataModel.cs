﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Arkivverket.Arkade.ExternalModels.Ead;
using Prism.Commands;
using Prism.Mvvm;

namespace Arkivverket.Arkade.UI.Models
{
    public class GuiMetaDataModel : BindableBase
    {


        private Visibility _itemVisibility = Visibility.Visible;

        private string _iconAdd = "PlusCircleOutline";
        private string _iconDelete = "Delete";
        private string _iconNameList = "MenuDown";

        public ICommand MyCommand { get; set; }


        public string IconAdd
        {
            get { return _iconAdd; }
            set { SetProperty(ref _iconAdd, value); }
        }

        public string IconDelete
        {
            get { return _iconDelete; }
            set { SetProperty(ref _iconDelete, value); }
        }

        public string IconNameList
        {
            get { return _iconNameList; }
            set { SetProperty(ref _iconNameList, value); }
        }

        public Visibility ItemVisibility
        {
            get { return _itemVisibility; }
            set { SetProperty(ref _itemVisibility, value); }
        }



        private string _archiveDescription;
        private string _agreementNumber;

        private string _entity;
        private string _contactPerson;
        private string _telephone;
        private string _email;

        private string _comment;

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }


        public bool ThisIsASystemEntry { get; set; }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }

        public string ContactPerson
        {
            get { return _contactPerson; }
            set { SetProperty(ref _contactPerson, value); }
        }

        public string Entity
        {
            get { return _entity; }
            set { SetProperty(ref _entity, value); }
        }

        private string _systemName;
        private string _systemVersion;
        private string _systemType;
        private string _systemTypeVersion;

        public string SystemName
        {
            get { return _systemName; }
            set { SetProperty(ref _systemName, value); }
        }
        public string SystemVersion
        {
            get { return _systemVersion; }
            set { SetProperty(ref _systemVersion, value); }
        }
        public string SystemType
        {
            get { return _systemType; }
            set { SetProperty(ref _systemType, value); }
        }
        public string SystemTypeVersion
        {
            get { return _systemTypeVersion; }
            set { SetProperty(ref _systemTypeVersion, value); }
        }





        public string ArchiveDescription
        {
            get { return _archiveDescription; }
            set { SetProperty(ref _archiveDescription, value); }
        }
        public string AgreementNumber
        {
            get { return _agreementNumber; }
            set { SetProperty(ref _agreementNumber, value); }
        }


        public GuiMetaDataModel(string archiveDescription, string agreementNumber)
        {
            ArchiveDescription = archiveDescription;
            AgreementNumber = agreementNumber;
            MyCommand = new DelegateCommand(MyCommandExecute);
        }


        public GuiMetaDataModel(string entity, string contactPerson, string telephone, string email)
        {
            Entity = entity;
            ContactPerson = contactPerson;
            Telephone = telephone;
            Email = email;

            MyCommand = new DelegateCommand(MyCommandExecute);
        }

        public GuiMetaDataModel(string systemName, string systemVersion, string systemType, string systemTypeVersion, bool thisIsASystemEntry)
        {
            SystemName = systemName;
            SystemVersion = systemVersion;
            SystemVersion = systemType;
            SystemTypeVersion = systemTypeVersion;

            ThisIsASystemEntry = thisIsASystemEntry;

            MyCommand = new DelegateCommand(MyCommandExecute);
        }


        public GuiMetaDataModel(string comment)
        {
            Comment = comment;

            MyCommand = new DelegateCommand(MyCommandExecute);
        }



        public void MyCommandExecute()
        {
            ItemVisibility = Visibility.Collapsed;
        }


}
}