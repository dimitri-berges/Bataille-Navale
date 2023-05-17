using GameCore;
using Spectre.Console;
using System;
using System.Collections.Generic;

namespace InterfaceConsole
{
    internal static class Utils
    {
        public static readonly Dictionary<CaseStatut, char> statutsDeCase = new()
        {
            { CaseStatut.Water, '.' },
            { CaseStatut.Boat, '.' },
            { CaseStatut.WaterHit, 'X' },
            { CaseStatut.BoatHit, '+' },
            { CaseStatut.BoatDrowned, '#' },
        };
        public static readonly Dictionary<CaseBoatType, char> typesDeBateau = new()
        {
            { CaseBoatType.None, '.' },
            { CaseBoatType.EdgeUp, 'A' },
            { CaseBoatType.EdgeDown, 'V' },
            { CaseBoatType.EdgeLeft, '<' },
            { CaseBoatType.EdgeRight, '>' },
            { CaseBoatType.Vertical, '|' },
            { CaseBoatType.Horizontal, '-' },
        };
        public static readonly Dictionary<char, string> markupsDeChars = new()
        {
            {statutsDeCase[CaseStatut.Water], $"[aqua]{statutsDeCase[CaseStatut.Water]}[/]" },
            {statutsDeCase[CaseStatut.WaterHit], $"[blue]{statutsDeCase[CaseStatut.WaterHit]}[/]" },
            {statutsDeCase[CaseStatut.BoatHit], $"[darkorange]{statutsDeCase[CaseStatut.BoatHit]}[/]" },
            {statutsDeCase[CaseStatut.BoatDrowned], $"[red]{statutsDeCase[CaseStatut.BoatDrowned]}[/]" },
            {typesDeBateau[CaseBoatType.EdgeUp], $"[yellow]{typesDeBateau[CaseBoatType.EdgeUp]}[/]" },
            {typesDeBateau[CaseBoatType.EdgeDown], $"[yellow]{typesDeBateau[CaseBoatType.EdgeDown]}[/]" },
            {typesDeBateau[CaseBoatType.EdgeLeft], $"[yellow]{typesDeBateau[CaseBoatType.EdgeLeft]}[/]" },
            {typesDeBateau[CaseBoatType.EdgeRight], $"[yellow]{typesDeBateau[CaseBoatType.EdgeRight]}[/]" },
            {typesDeBateau[CaseBoatType.Vertical], $"[yellow]{typesDeBateau[CaseBoatType.Vertical]}[/]" },
            {typesDeBateau[CaseBoatType.Horizontal], $"[yellow]{typesDeBateau[CaseBoatType.Horizontal]}[/]" },
        };
        public static int NbDigits(int value)
        {
            return ((value == 0) ? 1 : ((int)Math.Floor(Math.Log10(Math.Abs(value))) + 1));
        }

        public static char GetCharOfCase(Case @case)
        {
            return @case.Statut == CaseStatut.Boat ? typesDeBateau[@case.BoatType] : statutsDeCase[@case.Statut];
        }

        public static string GetMarkupForChar(char ch)
        {
            return markupsDeChars[ch];
        }
    }
}