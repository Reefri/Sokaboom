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

		public enum TestConnexionResult
		{
			Valid,
			NotFound,
			Incorrect
		}
		

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




        public TestConnexionResult TestConnexion(string pId,string pPassWord)
		{
            List<Account> lAccountList = JsonReaderWriter.ReadJsonToList<Account>(JSONFILE_PATH);

			int lIndex = lAccountList.FindIndex(lPredicate =>  lPredicate.Id == pId);

			if (lIndex >= 0)
			{
				if (lAccountList[lIndex].Password == GD.Hash(pPassWord))
				{
					currentAccount = lAccountList[lIndex];
					return TestConnexionResult.Valid;
				}
				else
				{
					return TestConnexionResult.Incorrect;
				}
			}
				

			return TestConnexionResult.NotFound;
		}

		public bool Register(string pId, string pPassWord)
		{
            List<Account> lAccountList = JsonReaderWriter.ReadJsonToList<Account>(JSONFILE_PATH);

            int lIndex = lAccountList.FindIndex(lPredicate => lPredicate.Id == pId);


            if (lIndex >= 0)
            {
                return false;
            }

			lAccountList.Add(Account.Create(pId,GD.Hash(pPassWord)));

			
			JsonReaderWriter.WirteListToJson(JSONFILE_PATH, lAccountList);

			return true;
        }


		
	

	}
}
