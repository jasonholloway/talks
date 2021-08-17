#!/bin/bash

([[ ! -e master.zip ]] &&
     wget https://github.com/hakimel/reveal.js/archive/master.zip &&
     unzip master.zip &&
     ln -s reveal.js-master reveal.js)

(cd reveal.js/dist/theme &&
     wget https://revealjs-themes.dzello.com/css/theme/sunblind.css)

(cd reveal.js/plugin &&
     git clone git@github.com:burnpiro/reveal-pointer.git)

(cd reveal.js/plugin &&
     git clone git@github.com:burnpiro/reveal-drawer.git)

(git clone git@github.com:mobomo/sketch.js.git)

(git clone git@github.com:csev/dazzleSketch.git &&
     find dazzleSketch -name '*js' | xargs sed -i 's/keypress/keydown/')

(git clone git@github.com:JeremyJeanson/prismjs-vs.git &&
     sed -i 's/code/pre/g' ./prismjs-vs/Sources/dist/prism-vs-dark.css )

(cd reveal.js/plugin &&
     git clone git@github.com:rajgoel/reveal.js-plugins.git)
     
