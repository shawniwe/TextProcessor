using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TextProcessor.Abstract.Interfaces;
using TextProcessor.Application.Menu;
using TextProcessor.Models;

namespace TextProcessor.Application
{
    // Класс приложения
    public class Application
    {
        private readonly IRepository<Word> wordRepository;
        private readonly ConsoleMenu menu;
        public Application(IRepository<Word> wordRepository)
        {
            this.wordRepository = wordRepository;
            menu = new (new ConsoleMenuItem[]
            {
                new ConsoleMenuItem()
                {
                    Name = "1. Загрузить файл в базу данных",
                    SelectedBackgroundColor = ConsoleColor.DarkCyan
                },
                new ConsoleMenuItem()
                {
                    Name = "2. Отобразить таблицу со словами из базы данных",
                    SelectedBackgroundColor = ConsoleColor.DarkCyan
                },
                new ConsoleMenuItem()
                {
                    Name = "3. Очистить базу данных",
                    SelectedBackgroundColor = ConsoleColor.DarkCyan
                },
            });

            menu.OnCurrentItemSelected += HandleConsoleBehavior;
            ConsoleMenuController menuController = new (menu);
            Console.CursorVisible = false;

            // infinity cycle
            while (true)
            {
                Console.Clear();
                foreach (var item in menu.MenuItems)
                {
                    if (menu.IsCurrentItem(item))
                    {
                        Console.ForegroundColor = ((ConsoleMenuItem)item).SelectedForegroundColor;
                        Console.BackgroundColor = ((ConsoleMenuItem)item).SelectedBackgroundColor;
                    }
                    Console.WriteLine(item.Name);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                var key = Console.ReadKey();
                menuController.OnKeyPressed?.Invoke(key.Key);
            }
        }

        private void HandleConsoleBehavior()
        {
            /*
             * Поведние консоли при выборе того или иного элемента меню
             */
            Console.Clear();
            switch (menu.MenuItems.ToList().IndexOf(menu.CurrentItem))
            {
                case 0:
                    Console.Write("Укажите путь к файлу > ");
                    string filePath = Console.ReadLine();
                    string sourceText = string.Empty;
                    try
                    {
                        sourceText = File.ReadAllText(filePath, Encoding.UTF8);
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка! Указан неверный путь, нажмите Enter для продолжения..");
                        Console.ReadKey();
                        return;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка! Указан неверный путь, нажмите Enter для продолжения..");
                        Console.ReadKey();
                        return;
                    }
                    catch (FileNotFoundException ex)
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка! Файл не найден, нажмите Enter для продолжения..");
                        Console.ReadKey();
                        return;
                    }
                    catch (SecurityException ex)
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка! Недостаточно прав для чтения этого файла, нажмите Enter для продолжения..");
                        Console.ReadKey();
                        return;
                    }
                    string[] words = sourceText.Split(' ');
                    /*
                     * Бизнес-логика:
                     * длина слова не менее 3 и не более 20 символов;
                     * слово упоминается в текущем входном файле не менее 4-ёх раз.
                     */
                    var wordModelList = words
                        .Where(x => x.Length >= 3 && x.Length <= 20 && words.Count(y => y == x) >= 4)
                        .Distinct()
                        .Select(x => new WordModel() { WordString = x, RepeatsCount = words.Count(y => y == x)});

                    foreach (var word in wordModelList)
                    {

                        /*
                        * если слово есть, обновляем запись в бд, если нет - создаем новую
                        * Использую транзакцию, чтобы при работе двух приложений одновременно,
                        * они работали только с закомиченными данными
                        */

                        var entry = wordRepository.ReadAll().FirstOrDefault(x => x.WordString == word.WordString);
                        if (entry == null)
                        {
                            wordRepository.BeginTransaction(IsolationLevel.ReadCommitted);
                            wordRepository.Create(new Word()
                            {
                                WordString = word.WordString,
                                RepeatsCount = word.RepeatsCount
                            });
                            wordRepository.CommitTransaction();

                        }
                        else
                        {
                            wordRepository.BeginTransaction(IsolationLevel.ReadCommitted);
                            entry.RepeatsCount += word.RepeatsCount;
                            wordRepository.Update(entry);
                            wordRepository.CommitTransaction();
                        }
                    }

                    Console.WriteLine("Данные успешно были записаны в базу данных! Нажмите Enter для продолжения..");
                    Console.ReadKey();
                    break;
                case 1:
                    if (!wordRepository.ReadAll().Any())
                    {
                        Console.WriteLine("Элементы в базе отсутствуют! Нажмите Enter для продолжения..");
                        Console.ReadKey();
                        break;
                    }
                    foreach (var element in wordRepository.ReadAll().OrderByDescending(x => x.RepeatsCount))
                    {
                        Console.WriteLine(element);
                    }
                    Console.ReadKey();
                    break;
                case 2:
                    Console.WriteLine("Элементы были успешно удалены! Нажмите Enter для продолжения..");
                    wordRepository.DeleteAll();
                    Console.ReadKey();
                    break;
            }
        }
    }
}