
# Project Work 3H - 2023/24
Realizzazione di programma C# + WPF, per giocare un solitario basato su carte da gioco personalizzate.

## Team di sviluppo
Questo progetto è sviluppato da:

- **Agostini** Alan
- **Angiolillo** Matteo
- **Balducci** Marco
- **Bovo** Alan Davide


## Gioco
Il gioco da sviluppare è il [Solitario a Croce](https://www.youtube.com/watch?v=g7TJviLmuMg)

**Come si gioca:** 5 carte del mazzo vengono posizionate al centro del tavolo a formare una croce. Sono lasciate libere 4 postazioni dette basi.

Scopo del solitario è costruire e completare le basi (dall’asso al re per ciascun seme, in senso ascendente), trasferendovi tutte le carte. Mentre per le basi vale la regola dello stesso seme in senso ascendente, nella croce vale quella del seme diverso in senso discendente.

La prima carta di ciascun mazzetto che forma la croce può andare alle basi o essere spostata su un posto vuoto o su di un’altra carta della croce. Si può spostare una sola carta alla volta. La carta del pozzo può andare direttamente alle basi o essere trasferita al tavolo.

**Tipo di carte:** carte italiane (4 semi x 10 carte per ogni seme).

## Aggiornare branch
+ Aggiungere il remote branch: `git remote add [nome] [url|ssh]`
+ Verificare se il remote è stato aggiunto: `git remote` o `git remote -v`

### Aggiornare github -> gitlab
+ Aggiornare il remote in locale: `git remote update`
+ Pullare le modifiche del remote: `git pull [nome] [branch]`
+ Aggiornare il gitlab: `git push`

### Aggiornare gitlab -> github
+ Pushare le modifiche sul remote: `git push [nome]`