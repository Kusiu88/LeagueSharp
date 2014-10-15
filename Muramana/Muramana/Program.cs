﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
namespace Muramana
{
    class Program
    {
        public static int Muramana = 3042;
        private static Menu Menu;
        private static bool hasAttacked;
        private static float distance = 0f;
        private static Obj_SpellMissile miss;
        private static Obj_AI_Hero target1;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Menu = new Menu("Muramana Activator", "MMAct", true);
            Menu.AddItem(new MenuItem("useM", "Use Muramana Activator").SetValue(true));
            Game.PrintChat("Muramana Activator By DZ191 Loaded.");
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            GameObject.OnCreate += Obj_SpellMissile_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
        }

        static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
 	        if (sender is Obj_SpellMissile && sender.IsValid)
            {
                var missile = (Obj_SpellMissile) sender;
                if(missile == miss)
                {
                    
                    int Mur = Items.HasItem(Muramana) ? 3042 : 3043;
                    if (target1.IsValid && ObjectManager.Get<Obj_AI_Hero>().Contains(target1) && (Items.HasItem(Mur)) && (Items.CanUseItem(Mur)) && (Menu.Item("useM").GetValue<bool>()))
                    {
                        Items.UseItem(Mur);
                    }
                    miss = null;
                    target1 = null;
                }
            }
        }

        private static void Obj_SpellMissile_OnCreate(GameObject sender, EventArgs args)
        {
 	         if (sender is Obj_SpellMissile && sender.IsValid)
            {
                var missile = (Obj_SpellMissile) sender;
                if (missile.SpellCaster is Obj_AI_Hero && missile.SpellCaster.IsValid &&
                    Orbwalking.IsAutoAttack(missile.SData.Name))
                {
                    miss = missile;
                }
            }
        }

        static void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if(unit.IsMe && (target is Obj_AI_Hero))
            {
                target1 = (Obj_AI_Hero)target;
            }
        }
        static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            int Mur = Items.HasItem(Muramana) ? 3042 : 3043;
            if (ObjectManager.Get<Obj_AI_Hero>().Contains(args.Target) && (Items.HasItem(Mur)) && (Menu.Item("useM").GetValue<bool>()) && (Items.CanUseItem(Mur)))
            {
                Items.UseItem(Mur);
            }
        }   
    }
}