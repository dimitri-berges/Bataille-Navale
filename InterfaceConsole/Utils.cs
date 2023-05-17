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
            {'.', "[aqua].[/]" },
            {'X', "[blue]X[/]" },
            {'+', "[red]+[/]" },
            {'#', "[red]#[/]" },
            {'A', "[yellow]A[/]" },
            {'V', "[yellow]V[/]" },
            {'<', "[yellow]<[/]" },
            {'>', "[yellow]>[/]" },
            {'|', "[yellow]|[/]" },
            {'-', "[yellow]-[/]" },
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