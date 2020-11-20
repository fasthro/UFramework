#!/bin/bash
CUR_DIR=$(dirname $(readlink -f $0))
bash "$CUR_DIR/center.sh" stop
sleep 1
bash "$CUR_DIR/login.sh" stop
sleep 1
bash "$CUR_DIR/game.sh" stop