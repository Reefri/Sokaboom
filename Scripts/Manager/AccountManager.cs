using Godot;
using System;
using System.Collections.Generic;
using GodotDict = Godot.Collections.Dictionary;
using System.Text.Json;
using System.IO;



// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class AccountManager 
	{
		static private AccountManager instance;

        private const string JSONFILE_PATH = "Accounts.json";

		public Account currentAccount;

        private AccountManager():base() 
		{
			if (instance != null)
			{
				GD.Print(nameof(AccountManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public AccountManager GetInstance()
		{
			if (instance == null) instance = new AccountManager();
			return instance;
		}




        public bool TestConnexion(string pId,string pPassWord)
		{
            List<Account> lAccountList = JsonReaderWriter.ReadJsonToList<Account>(JSONFILE_PATH);

			int lIndex = lAccountList.FindIndex(lPredicate =>  lPredicate.Id == pId);

			if (lIndex >= 0)
			{
				if (lAccountList[lIndex].Password == GD.Hash(pPassWord))
				{
					currentAccount = lAccountList[lIndex];
					GD.Print("Connexion réussie !");
					return true;
				}
				else
				{
					GD.Print("Mot de passe incorrecte !");
					return false;
				}
			}
				

			GD.Print("Aucun compte n'a été trouvé avec le le pseudo :" +  pId);
			return false;
		}

		public void Register(string pId, string pPassWord)
		{
            List<Account> lAccountList = JsonReaderWriter.ReadJsonToList<Account>(JSONFILE_PATH);

            int lIndex = lAccountList.FindIndex(lPredicate => lPredicate.Id == pId);


            if (lIndex >= 0)
            {
                GD.Print("Un compte existe déjà avec ce pseudo.");
                return;
            }

			lAccountList.Add(Account.Create(pId,GD.Hash(pPassWord)));

			
			JsonReaderWriter.WirteListToJson(JSONFILE_PATH, lAccountList);

        }


		
	

	}
}
