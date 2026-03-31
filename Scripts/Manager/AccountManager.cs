using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;
using GodotDict = Godot.Collections.Dictionary;
using System.Linq;


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

			int lIndex = lAccountList.FindIndex(lAccount =>  lAccount.Id == pId);

			if (lIndex >= 0)
			{
				if (lAccountList[lIndex].Password == GD.Hash(pPassWord))
				{
					currentAccount = lAccountList[lIndex];
					currentAccount.Update();
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

			
			JsonReaderWriter.WriteListToJson(JSONFILE_PATH, lAccountList);

			return true;
        }


		public void UpdateAccount()
		{

            List<Account> lAccountList = JsonReaderWriter.ReadJsonToList<Account>(JSONFILE_PATH);

            int lIndex = lAccountList.FindIndex(lPredicate => lPredicate.Id == currentAccount.Id);


            if (lIndex < 0)
            {

				GD.Print("Joueur courrant non trouvé ? Quelque chose s'est vraiment très mal passé...");
				GD.Print("Connexion au compte Guest : " + (TestConnexion("Guest","") == TestConnexionResult.Valid));
                return ;
            }

			lAccountList[lIndex] = currentAccount;


            JsonReaderWriter.WriteListToJson(JSONFILE_PATH, lAccountList);
        }

		

		public void NewWin(int pScore, int pPar)
		{

			bool lDidModif= false;

			if (currentAccount.Score[GridManager.GetInstance().CurrentLevelIndex] < pScore)
			{
				currentAccount.Score[GridManager.GetInstance().CurrentLevelIndex] = pScore;
				currentAccount.BestPar[GridManager.GetInstance().CurrentLevelIndex] = pPar;

				lDidModif = true;
			}

			int lIndex = Mathf.Clamp(GridManager.GetInstance().CurrentLevelIndex + 1, 0, GridManager.GetInstance().numberOfLevel - 1);

			if (!currentAccount.LockedLevels[lIndex]) { currentAccount.LockedLevels[lIndex] = true; lDidModif = true; }

			if (lDidModif) 
            UpdateAccount();

        }
	

		public List<Account> GetTopPlayers(int pNumberOfPlayer)
		{

            List<Account> lAccountList = JsonReaderWriter.ReadJsonToList<Account>(JSONFILE_PATH);

			return lAccountList.OrderByDescending(lAccount => lAccount.FinalScore()).Take(pNumberOfPlayer).ToList();
        }


    }
}
