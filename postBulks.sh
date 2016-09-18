#/bin/bash
for i in {0..1440}
do
    curl -XPOST 'localhost:9200/words/word/_bulk' --data-binary "@bulks/bulk-$i.txt"
done