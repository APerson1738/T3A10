﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Trainer
{
    public class TrainerBehaviour : ModBehaviour
    {
        public static bool ModActive = false;
        public static bool LockAge = false;
        public static bool LockStress = false;
        public static bool LockHunger = false;
        public static bool LockNeeds = false;
        public static bool LockEffSat = false;
        public static bool FreeEmployees = false;
        public static bool FreeStaff = false;
        public static bool TempLock = false;
        public static bool NoWaterElect = false;
        public static bool NoiseRed = false;
        public static bool FullEnv = false;
        public static bool CleanRooms = false;
        public static bool Fullbright = false;
        public static bool NoVacation = false;

        public static string CompanyText = "";
        public static string price_ProductName = "";
        public static float price_ProductPrice = 10f;

        public Text button = WindowManager.SpawnLabel();
        public bool start = false;
        void Start()
        {
            //All ModBehaviours has a function to load settings from the mod's settings file
            //Note that everything is saved in strings
            //This function uses the default string converter for the generic type argument
            //if (ModActive && GameSettings.Instance != null && HUD.Instance != null) { }
            //this.ModActive = this.LoadSetting<bool>("ModActive", false);
            if (ModActive)
            {
                StartCoroutine(Spremi());
                LockAge = this.LoadSetting<bool>("LockAge", false);
                LockStress = this.LoadSetting<bool>("LockStress", false);
                LockNeeds = this.LoadSetting<bool>("LockNeeds", false);
                FreeEmployees = this.LoadSetting<bool>("FreeEmployees", false);
                LockEffSat = this.LoadSetting<bool>("LockEffSat", false);
                FreeStaff = this.LoadSetting<bool>("FreeStaff", false);
                TempLock = this.LoadSetting<bool>("TempLock", false);
                NoWaterElect = this.LoadSetting<bool>("NoWaterElect", false);
                NoiseRed = this.LoadSetting<bool>("NoiseRed", false);
                FullEnv = this.LoadSetting<bool>("FullEnv", false);
                CleanRooms = this.LoadSetting<bool>("CleanRooms", false);
                Fullbright = this.LoadSetting<bool>("Fullbright", false);
                NoVacation = this.LoadSetting<bool>("NoVacation", false);
                LoanWindow.factor = 250000;
                GameSettings.MaxFloor = 25; 
            }
        }
        void Update()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                if(this.start == false)
                {
                    Main.Tipka();
                    this.start = true;
                }
                //GameSettings.GameSpeed = 10f;
                if (FreeStaff)
                {
                    GameSettings.Instance.StaffSalaryDue = 0f;
                }
                foreach (var stvar in GameSettings.Instance.sRoomManager.AllFurniture)
                {
                    if (NoiseRed)
                    {
                        stvar.ActorNoise = 0f;
                        stvar.EnvironmentNoise = 0f;
                        stvar.FinalNoise = 0f;
                        stvar.Noisiness = 0;
                    }
                    if (NoWaterElect)
                    {
                        stvar.Water = 0;
                        stvar.Wattage = 0;
                    } 
                }
                foreach (var soba in GameSettings.Instance.sRoomManager.Rooms)
                {
                    if (CleanRooms)
                    {
                        soba.ClearDirt();
                    }
                    if (TempLock)
                    {
                        soba.Temperature = 21f;
                    }
                    if (FullEnv)
                    {
                        soba.FurnEnvironment = 3;
                    }
                    if (Fullbright)
                    {
                        soba.IndirectLighting = 4;
                    }
                }
                foreach (var item in GameSettings.Instance.sActorManager.Actors)
                {
                    if (LockAge)
                    {
                        item.employee.AgeMonth = 20 * 12;
                        item.UpdateAgeLook();
                    }
                    if (LockStress)
                    {
                        item.employee.Stress = 1;
                    }
                    if (LockEffSat)
                    {
                        item.Effectiveness = 10;
                        item.ChangeSatisfaction(10, 10, Employee.Thought.LoveWork, Employee.Thought.LikeTeamWork, 0);
                    }
                    if (LockNeeds)
                    {
                        item.employee.Bladder = 1;
                        item.employee.Hunger = 1;
                        item.employee.Energy = 1;
                        item.employee.Social = 1;
                    }
                    if (FreeEmployees)
                    {
                        item.employee.Salary = 0;
                        item.NegotiateSalary = false;
                    }
                    if (NoiseRed)
                    {
                        item.Noisiness = 0;
                    }
                    if (NoVacation)
                    {
                        item.VacationMonth = SDateTime.NextMonth(60);
                    }
                }
            }
        }
        IEnumerator<WaitForSeconds> Spremi()
        {
            while (true)
            {
                yield return new WaitForSeconds(15.0f);
                SaveSetting("LockStress", LockStress.ToString());
                SaveSetting("NoVacation", NoVacation.ToString());
                SaveSetting("Fullbright", Fullbright.ToString());
                SaveSetting("CleanRooms", CleanRooms.ToString());
                SaveSetting("FullEnv", FullEnv.ToString());
                SaveSetting("NoiseRed", NoiseRed.ToString());
                SaveSetting("FreeStaff", FreeStaff.ToString());
                SaveSetting("TempLock", TempLock.ToString());
                SaveSetting("NoWaterElect", NoWaterElect.ToString());
                SaveSetting("LockNeeds", LockNeeds.ToString());
                SaveSetting("LockEffSat", LockEffSat.ToString());
                SaveSetting("FreeEmployees", FreeEmployees.ToString());
                SaveSetting("LockAge", LockAge.ToString());
            }
        }
        internal void ClearLoans()
        {
            GameSettings.Instance.Loans.Clear();
            HUD.Instance.AddPopupMessage("Trainer: All loans are cleared!", "Cogs", "", 0, 0, 0, 0, 1);
            //SoftwareProduct prod = new SoftwareProduct();
            //prod.Server = "ServerName";
            //prod.ServerReq = 0.9f;
        }
        public static void NoVacationBool()
        {
            if (NoVacation) NoVacation = false;
            else NoVacation = true;
        }
        public static void FullbrightBool()
        {
            if (Fullbright) Fullbright = false;
            else Fullbright = true;
        }
        public static void CleanRoomsBool()
        {
            if (CleanRooms) CleanRooms = false;
            else CleanRooms = true;
        }
        public static void FullEnvBool()
        {
            if (FullEnv) FullEnv = false;
            else FullEnv = true;
        }
        public static void NoiseRedBool()
        {
            if (NoiseRed) NoiseRed = false;
            else NoiseRed = true;
        }
        public static void FreeStaffBool()
        {
            if (FreeStaff) FreeStaff = false;
            else FreeStaff = true;
        }
        public static void TempLockBool()
        {
            if (TempLock) TempLock = false;
            else TempLock = true;
        }
        public static void NoWaterElectBool()
        {
            if (NoWaterElect) NoWaterElect = false;
            else NoWaterElect = true;
        }
        public static void LockStressOfEmployees()
        {
            if (LockStress) LockStress = false;
            else LockStress = true;
        }
        public static void DisableNeeds()
        {
            if (LockNeeds) LockNeeds = false;
            else LockNeeds = true;
        }
        public static void LockEmpSal()
        {
            if (FreeEmployees) FreeEmployees = false;
            else FreeEmployees = true;
        }
        public static void FullEffSat()
        {
            if (LockEffSat) LockEffSat = false;
            else LockEffSat = true;
        }
        public static void LockAgeOfEmployees()
        {
            if (LockAge) LockAge = false;
            else LockAge = true;
        }

        public static void SetProductPrice()
        {
          foreach (SoftwareProduct product in GameSettings.Instance.MyCompany.Products)
          {
            if (product.Name == price_ProductName)
            {
                product.Price = price_ProductPrice;
                HUD.Instance.AddPopupMessage("Trainer: Price for " + product.Name + " has been setted up!", "Cogs", "", 0, 0, 0, 0, 1);
                break;
            }
          }
        }

        public static void SetProductStock()
        {
            foreach (SoftwareProduct product in GameSettings.Instance.MyCompany.Products)
            {
                if (product.Name == price_ProductName) //&& product.Userbase < 500)
                {
                    //var st = Convert.ToInt32(product.PhysicalCopies) * (Convert.ToInt32(product.Price) / 2);
                    product.PhysicalCopies = (uint)price_ProductPrice;
                    HUD.Instance.AddPopupMessage("Trainer: Stock for " + product.Name + " has been setted up!", "Cogs", "", 0, 0, 0, 0, 1);
                    //GameSettings.Instance.MyCompany.MakeTransaction(st, Company.TransactionCategory.Sales);
                    break;
                }
            }
        }

        public static void AddActiveUsers()
        {
            foreach (SoftwareProduct product in GameSettings.Instance.MyCompany.Products)
            {
                if (product.Name == price_ProductName)
                {
                    product.Userbase = Convert.ToInt32(price_ProductPrice);
                    HUD.Instance.AddPopupMessage("Trainer: Added " + Convert.ToInt32(price_ProductPrice) + " active users to product " + product.Name, "Cogs", "", 0, 0, 0, 0, 1);
                    break;
                }
            }
        }
        
        public static void RemoveSoft()
        {
            SimulatedCompany kompanija = new SimulatedCompany();
            foreach (SoftwareProduct product in GameSettings.Instance.MyCompany.Products)
            {
                //kompanija.CanMakeTransaction(10000000000f);
                //if (product.Inventor != GameSettings.Instance.MyCompany.Name)
                using (IEnumerator<SoftwareProduct> enumerator = (IEnumerator<SoftwareProduct>)GameSettings.Instance.MyCompany.Products.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                        enumerator.Current.Trade(kompanija);
                }
                //kompanija.Products.Add(product);
                //product.DevCompany = kompanija;
                
            }
        }
        public static void TakeoverCompany()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                foreach (KeyValuePair<uint, SimulatedCompany> company in GameSettings.Instance.simulation.Companies)
                {
                    SimulatedCompany simulatedCompany = company.Value;
                    if (simulatedCompany.Name == CompanyText)
                    {
                        simulatedCompany.BuyOut(GameSettings.Instance.MyCompany, true);
                        HUD.Instance.AddPopupMessage("Trainer: Company " + simulatedCompany.Name + " has been takovered by you!", "Cogs", "", 0, 0, 0, 0, 1);
                        break;
                    }
                }
            }
        }
        public static void SubDCompany()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                foreach (KeyValuePair<uint, SimulatedCompany> company in GameSettings.Instance.simulation.Companies)
                {
                    SimulatedCompany simulatedCompany = company.Value;
                    if (simulatedCompany.Name == CompanyText)
                    {
                        simulatedCompany.MakeSubsidiary(GameSettings.Instance.MyCompany);
                        simulatedCompany.IsSubsidiary();
                        HUD.Instance.AddPopupMessage("Trainer: Company " + simulatedCompany.Name + " is now your subsidiary!", "Cogs", "", 0, 0, 0, 0, 1);
                        break;
                    }
                }
            }
        }
        public static void IncreaseMoney()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                GameSettings.Instance.MyCompany.MakeTransaction(Main.NovacBox.ConvertToInt(Main.NovacBox), Company.TransactionCategory.Deals);
                HUD.Instance.AddPopupMessage("Trainer: Money has been added in category Deals!", "Cogs", "", 0, 0, 0, 0, 1);
            }
        }

        public void ResetAgeOfEmployees()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                foreach (var item in GameSettings.Instance.sActorManager.Actors)
                {
                    item.employee.AgeMonth = 20 * 12;
                    item.UpdateAgeLook();
                }
                HUD.Instance.AddPopupMessage("Trainer: Age of employees has been reset!", "Cogs", "", 0, 0, 0, 0, 1);
            }
        }

        public override void OnActivate()
        {
            ModActive = true;
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                HUD.Instance.AddPopupMessage("Trainer v2 has been activated!", "Cogs", "", 0, 0, 0, 0, 1);
            }
        }

        internal static void AddRep()
        {
            if (!((UnityEngine.Object)GameSettings.Instance != (UnityEngine.Object)null))
                return;
            GameSettings.Instance.MyCompany.BusinessReputation = 1f;
            SoftwareType random1 = Utilities.GetRandom<SoftwareType>(Enumerable.Where<SoftwareType>((IEnumerable<SoftwareType>)GameSettings.Instance.SoftwareTypes.Values, (Func<SoftwareType, bool>)(x => !x.OneClient)));
            string random2 = Utilities.GetRandom<string>((IEnumerable<string>)random1.Categories.Keys);
            GameSettings.Instance.MyCompany.AddFans(Main.RepBox.ConvertToInt(Main.RepBox), random1.Name, random2);
            HUD.Instance.AddPopupMessage("Trainer: Reputation has been added for SoftwareType: "+random1.Name + ", Category: "+random2, "Cogs", "", 0, 0, 0, 0, 1);
        }

        public override void OnDeactivate()
        {
            ModActive = false;
            if (!ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                HUD.Instance.AddPopupMessage("Trainer v2 has been deactivated!", "Cogs", "", 0, 0, 0, 0, 1);
            }
        }

        internal void EmployeesToMax()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                if (!((UnityEngine.Object)SelectorController.Instance != (UnityEngine.Object)null))
                    return;
                foreach (var x in GameSettings.Instance.sActorManager.Actors)
                {
                    //x.HREd = true;
                    for (int index = 0; index < 5; ++index)
                    {
                        x.employee.ChangeSkill((Employee.EmployeeRole)index, 1f, false);
                        foreach (string specialization in GameSettings.Instance.Specializations)
                        {
                            x.employee.AddToSpecialization(Employee.EmployeeRole.Designer, specialization, 1f, 0, true);
                            x.employee.AddToSpecialization(Employee.EmployeeRole.Artist, specialization, 1f, 0, true);
                            x.employee.AddToSpecialization(Employee.EmployeeRole.Programmer, specialization, 1f, 0, true);
                        }
                    }
                }
                HUD.Instance.AddPopupMessage("Trainer: All employees are now max skilled!", "Cogs", "", 0, 0, 0, 0, 1);
            }
        }

        internal void UnlockAllSpace()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                if (!((UnityEngine.Object)GameSettings.Instance != (UnityEngine.Object)null))
                    return;
                GameSettings.Instance.BuildableArea = new Rect(9f, 1f, 246f, 254f);
                GameSettings.Instance.ExpandLand(0, 0);
                HUD.Instance.AddPopupMessage("Trainer: All buildable area is now unlocked!", "Cogs", "", 0, 0, 0, 0, 1);
            }
        }

        internal void UnlockAll()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                Cheats.UnlockFurn = !Cheats.UnlockFurn;
                if (!((UnityEngine.Object)HUD.Instance != (UnityEngine.Object)null))
                    return;
                HUD.Instance.UpdateFurnitureButtons();
                HUD.Instance.AddPopupMessage("Trainer: All furniture has been unlocked!", "Cogs", "", 0, 0, 0, 0, 1);
            }
        }

    }
    public static class Extensions
    {
        /// <summary>
        /// Sets a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is set</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">Value to set.</param>
        /// <returns>PropertyValue</returns>
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            Type t = obj.GetType();
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }
    }
}
