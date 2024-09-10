# Ce que fait ce `README.md` :

- **Caract�ristiques** : D�crit ce que fait le microservice `ms-log`.
- **Pr�requis** : �num�re les outils n�cessaires pour construire et ex�cuter le service.
- **Installation** : Fournit des instructions pour cloner le d�p�t et acc�der au r�pertoire.
- **Configuration** : Explique que le microservice utilise une base de donn�es.
- **Construire et Ex�cuter le Microservice** : Explique comment construire et ex�cuter le service localement et via Docker.
- **Utilisation** : Explique comment utiliser les points de terminaison OData pour interagir avec le service.
- **Exemples de Requ�tes avec Postman** : Fournit des exemples concrets de requ�tes pour tester le service.
- **Contribuer** : Encouragement � contribuer au projet.
- **License** : D�crit la licence sous laquelle le projet est distribu�.

## Caract�ristiques
`ms-log` est un microservice bas� sur .NET Minimal APIs et OData qui sert de point central pour g�rer les logs des appplications dans une architecture orient�e �v�nements (Event-Driven Architecture). Ce microservice permet de g�rer et de r�cup�rer les logs des diff�rents microservices � travers une API REST OData.

- **OData API** : Fournit une API RESTful utilisant OData pour g�rer les logs.
- **Minimal APIs** : Utilise le mod�le l�ger de Minimal APIs de .NET pour une configuration plus simple.
- **SQlite Database** : Utilise une base de donn�es SQLite pour stocker les logs.
- **Containerisation** : Conteneuris� � l'aide de Docker pour un d�ploiement facile.

## Pr�requis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

## Installation

Clonez ce d�p�t et acc�dez au r�pertoire du projet :

```bash
git clone https://github.com/brendanGiraudet/ms-log.git
cd ms-log
```

## Configuration
Il n'y a pas de configuration n�cessaire pour le d�veloppement, car le microservice utilise la migration de base de donn�e.

Construire et Ex�cuter le Microservice

1. Construire le Projet
Assurez-vous que vous avez le .NET 8 SDK install�, puis construisez le projet :

```bash
Copier le code
dotnet build
```

2. Ex�cuter le Projet Localement
Ex�cutez le microservice localement pour le tester :

```bash
Copier le code
dotnet run
```

Le microservice sera disponible � l'adresse http://localhost:5000 (port specifi� dans le fichier appsettings).

3. Utiliser Docker
Construire l'Image Docker et/ou l'�x�cuter
Pour construire l'image Docker du microservice, utilisez la commande suivante :

```bash
Copier le code
docker compose -f .\docker-compose-debug.yml up
```

Le service sera disponible � http://localhost:1818 (port specifi� dans le fichier docker-compose-debug).

## Utilisation
Points de Terminaison OData
Le microservice expose plusieurs points de terminaison OData pour g�rer les logs.

GET /odata/Logs : R�cup�re toutes les Logs.
GET /odata/Logs({key}) : R�cup�re un Log sp�cifique par ID.
POST /odata/Logs : Ajoute un nouveau Log.

## Exemples de Requ�tes avec Postman
R�cup�rer tous les Logs
M�thode : GET
URL : http://localhost:8080/odata/Logs

Ajouter une nouvelle configuration RabbitMQ
M�thode : POST
URL : http://localhost:8080/odata/Logs
Corps (JSON) :
json
Copier le code
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "level": "string",
  "message": "string",
  "timestamp": "2024-09-09T16:16:59.465Z",
  "applicationCode": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "deleted": true
}

## Contribuer
Les contributions sont les bienvenues ! Veuillez soumettre une issue ou une pull request pour toute am�lioration ou probl�me.

## License
Aucune licence

