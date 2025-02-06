# WinHub

[![.NET](https://github.com/alainranger/WinHub/actions/workflows/dotnet.yml/badge.svg)](https://github.com/alainranger/WinHub/actions/workflows/dotnet.yml)

WinHub est une application de gestion de tirage au sort.  Il permet de créer un ou plusieurs tirages.  
Chaque tirage a ses participants et chaque participant peu avoir un ou plusieurs participations.

L'application sert à expérimenter la plateforme .NET Aspire.   Cette plateforme est un ensemble d’outils, 
de modèles et de packages permettant de créer des applications observables et prêtes pour la production.

Le back-end est une application monolithe ASP.NET Core.  Pour le back-end j’applique l’architecture de tranche vertical.

Le front-end est une application Blazor.  Ultimement, il y aura une version React, Angular et VueJS 
pour démontrer que .NET Aspire supporte d’autre plateforme que .NET.

Pour la couche base de données, j’utilise Entity Framework Core.  Le but est de pourvoir expérimenter 
plusieurs bases de données comme SQL Server, PostgreSQL et Maria DB sans changer le code.  
