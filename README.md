# 💎 Gestion de Pierres Précieuses

## 🧾 Présentation

Ce projet est une **application web complète de gestion de pierres précieuses**, développée en **ASP.NET Core (C#)**.  
Elle permet l'insertion, la vente et le suivi des pierres via une interface sécurisée et intuitive.

L'application utilise une **authentification par numéro de téléphone**, avec enregistrement des utilisateurs, suivi des ventes, gestion du stock, et une base de données PostgreSQL robuste.

---

## 🔐 Fonctionnalités principales

### 👤 Authentification & Inscription
- Inscription avec **numéro de téléphone valide**
- Connexion sécurisée via numéro
- Gestion des rôles (utilisateur, administrateur)

### 📦 Gestion de stock
- Ajout de nouvelles pierres dans le système
- Mise à jour des informations de chaque pierre (nom, type, poids, prix, etc.)
- Visualisation du stock en temps réel

### 💰 Vente de pierres
- Interface pour vendre des pierres disponibles
- Historique des ventes enregistrées
- Calcul automatique des montants et bénéfices

### 📈 Suivi & Historique
- Suivi complet des ventes effectuées
- Possibilité de filtrer les ventes par date ou type de pierre
- Visualisation des performances de vente

---

## ⚙️ Technologies utilisées

| Technologie        | Rôle                                      |
|--------------------|-------------------------------------------|
| **ASP.NET Core (C#)** | Backend et logique métier                |
| **PostgreSQL**     | Base de données relationnelle             |
| **HTML / CSS / Bootstrap** | Interface utilisateur responsive     |
| **JavaScript**     | Dynamisme et interactions côté client     |

---

## 📌 Objectifs du projet

- Offrir une solution complète pour gérer un **stock de pierres précieuses**
- Utiliser des **technologies modernes** et professionnelles (C#, PostgreSQL)
- Mettre en place une **authentification personnalisée** par numéro
- Suivre les performances de vente et les mouvements de stock

---

## 🚀 Lancer le projet en local

1. **Cloner le dépôt**
   ```bash
   git clone https:https://github.com/romeo2433/Stones1234.git
   cd nom-du-repo

Configurer la base PostgreSQL

Créer une base nommée stones 

Mettre à jour la chaîne de connexion dans appsettings.json

Lancer les migrations (si EF Core)
   ```bash
   dotnet ef database update
   dotnet run


