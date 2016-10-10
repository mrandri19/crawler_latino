# Crawler Latino
A crawling script for [www.dizionario-latino.com](http://www.dizionario-latino.com/)

- `data.fsx` will download the files and put them in the `data` folder

- `bulks.fsx` will parse the files and put them in the `bulks` folder in the elasticsearch bulk insert format

- `postBulks.sh` will POST the files in `bulks` to elastichsearch

- `collateWordsAndPUT.fsx` will remove the accents and PUT them to a local server
## A word looks like this 
`{"name":" ăbĕo (v. intr. an.)","link":"/dizionario-latino-italiano.php?lemma=ABEO100"}`
