# 🛠️ Installateur .NET Framework et C++ Redistributable

**Installateur .NET Framework et C++ Redistributable** est une application 🖥️ Windows en C# conçue pour faciliter l'installation des différentes versions de 📦 .NET Framework et des 🔧 Visual C++ Redistributables. Elle permet d'identifier les ⚙️ composants déjà installés et de télécharger et installer automatiquement les versions 🔄 compatibles avec votre système Windows 10/11.

![Screen](https://dotnetinstaller.o2cloud.fr.com/logo.png)

- ✅ Détection automatique des packages déjà installés 🔍
- ✅ Compatibilité Windows 10 et Windows 11 🏷️
- ✅ Installation groupée ou individuelle 📦
- ✅ Interface console intuitive et conviviale 🖥️
- ✅ Téléchargement automatique depuis les sources officielles 🔄
- ✅ Installation silencieuse des packages ⚙️

## 🚀 Fonctionnalités

- 🔍 Détection automatique des packages déjà installés sur le système.
- 🏷️ Identification des packages compatibles avec Windows 10 et Windows 11.
- 📦 Installation groupée de tous les packages compatibles.
- 🔧 Installation sélective d'un package spécifique.
- 📊 Installation groupée par catégorie (.NET Framework ou C++ Redistributable).
- 🧹 Nettoyage automatique des fichiers temporaires après utilisation.
- 🛡️ Téléchargement sécurisé depuis les serveurs Microsoft officiels.
- 🔄 Réinstallation optionnelle des packages déjà présents.

## 📋 Pré-requis

- 🏷️ Windows 10 ou Windows 11
- 🔧 .NET Framework 4.0 ou supérieur (pour exécuter l'application)
- 🔑 Droits administrateur
- 🌐 Connexion Internet pour le téléchargement des packages

## 📖 Utilisation

1. Lancer l'application en mode administrateur.
2. Consulter la liste des packages disponibles avec leur statut d'installation.
3. Choisir une option dans le menu principal :
   - 1️⃣ Installer tous les packages compatibles avec votre version de Windows
   - 2️⃣ Installer un package spécifique de votre choix
   - 3️⃣ Installer tous les C++ Redistributable compatibles
   - 4️⃣ Installer tous les .NET Framework compatibles
   - 5️⃣ Quitter l'application

4. Suivre les instructions à l'écran pendant le processus d'installation.
5. 🔄 Redémarrer l'ordinateur si nécessaire après les installations.

## 🔧 Packages inclus

### .NET Framework
- .NET Framework 3.5 SP1
- .NET Framework 4.5.2
- .NET Framework 4.6
- .NET Framework 4.6.1
- .NET Framework 4.6.2
- .NET Framework 4.7
- .NET Framework 4.7.1
- .NET Framework 4.7.2
- .NET Framework 4.8
- .NET Framework 4.8.1

### Visual C++ Redistributable
- Visual C++ 2005 (x86/x64)
- Visual C++ 2008 (x86/x64)
- Visual C++ 2010 (x86/x64)
- Visual C++ 2012 (x86/x64)
- Visual C++ 2013 (x86/x64)
- Visual C++ 2015-2022 (x86/x64)

## ⚙️ Paramètres d'installation

Tous les packages sont installés avec des options silencieuses pour minimiser les interactions utilisateur. Les arguments d'installation sont:

- .NET Framework : `/q /norestart`
- Visual C++ : `/passive /norestart` ou `/q` selon la version

## 📄 License

Ce projet est distribué sous la licence MIT.

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à soumettre des pull requests pour améliorer cet outil.

## 🙏 Remerciements

- Microsoft pour la mise à disposition des packages redistributables
- Tous les contributeurs qui ont aidé à améliorer cet outil
