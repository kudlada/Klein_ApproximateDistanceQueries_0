#!/bin/sh

# Pouziti: getOSMdata.sh 1 2 3
#
# Stahne data popsana v souborech 1, 2 a 3
# z "http://overpass-api.de/api/interpreter"
# a ulozi je do odpovidajicich souboru 
# 1.osm, 2.osm a 3.osm ve stejnem adresari.


for f in "$@";do
        [ -n "$f" ] || exit 1
        wget -O "$f.osm" --post-file="$f" "http://overpass-api.de/api/interpreter"
        done;
