﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RPGHelper.Data;
using RPGHelper.Models;
using RPGHelper.Services;

namespace RPGHelper.Client.Views
{
    /// <summary>
    /// Interaction logic for HeroesView.xaml
    /// </summary>
    public partial class HeroesView : UserControl
    {
        public HeroesView()
        {
            InitializeComponent();
            LoadData();

            //var hero1 = new Hero()
            //{
            //    Name = "Kunkka",
            //    Gold = 2000m
            //};

            //var hero2 = new Hero()
            //{
            //    Name = "Sven",
            //    Gold = 2000m
            //};

            //var hero3 = new Hero()
            //{
            //    Name = "Mirana",
            //    Gold = 2000m
            //};

            //var heroes = new List<Hero>();

            //heroes.Add(hero1);
            //heroes.Add(hero2);
            //heroes.Add(hero3);

            //foreach (var hero in heroes)
            //{
            //    var listHeroes = new ListViewItem();
            //    listHeroes.Content = hero.Name;
            //    HeroesList.Items.Add(listHeroes);
            //}

            //HeroesList.Items.Add(heroes);

            var context = new RPGHelperContext();

            var user = AuthenticationService.GetCurrentUser();

            var heroes = context.Heroes
                //.Where(h => h.UserId == user.Id)
                .OrderBy(h => h.Name)
                .ToList();

            var heroesList = new List<Hero>();

            foreach (var hero in heroes)
            {
                heroesList.Add(hero);
            }

            //this.DataContext = new List<Hero>(heroesList);
            //HeroesList.ItemsSource = heroesList;
        }

        public void LoadData()
        {
            var context = new RPGHelperContext();
            var user = AuthenticationService.GetCurrentUser();

            this.DataContext = new ObservableCollection<Hero>
                (
                    context.Heroes
                    //.Where(h => h.UserId == user.Id)
                    .OrderBy(h => h.Name)
                    .ToList()
                );
        }

        private void AddHero_Click(object sender, RoutedEventArgs e)
        {
            var context = new RPGHelperContext();
            var user = AuthenticationService.GetCurrentUser();
            
            var newHero = new Hero
            {
                Name = "New Hero",
                Gold = 0.00m,
                UserId = user.Id
            };

            var newStats = new HeroStats()
            {
                Hero = newHero,
                Hp = 0,
                Defence = 0,
                AttackPower = 0,
                Mana = 0
            };

            context.Heroes.Add(newHero);
            context.HeroStatistics.Add(newStats);

            ((ObservableCollection<Hero>)DataContext).Add(newHero);
        }

        private void EditHero_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteHero_Click(object sender, RoutedEventArgs e)
        {
            var context = new RPGHelperContext();
            Button b = sender as Button;
            var currentHeroId = (int)b.DataContext;
            var currentHero = context.Heroes.FirstOrDefault(h => h.Id == currentHeroId);

            var result = MessageBox.Show("Are you sure you want to delete this Hero?", "Question",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes && currentHero != null)
            {
                using (context = new RPGHelperContext())
                {
                    Hero heroToRemove = context.Heroes.FirstOrDefault(h => h.Id == currentHero.Id);
                    HeroStats heroStats = context.HeroStatistics.FirstOrDefault(hs => hs.Hero.Id == currentHero.Id);

                    if (heroStats != null)
                    {
                        context.HeroStatistics.Remove(heroStats);
                        context.SaveChanges();
                    }
                    if (heroToRemove != null)
                    {
                        context.Heroes.Remove(heroToRemove);
                        context.SaveChanges();
                    }
                }
                MessageBox.Show("Hero removed successfully!");
            }
            else
            {
                MessageBox.Show("This Hero is not added yet!");
            }

            LoadData();
        }
    }
}
