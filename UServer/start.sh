#!/bin/bash
CUR_DIR=$(dirname $(readlink -f $0))
bash "$CUR_DIR/login.sh" start
sleep 1
bash "$CUR_DIR/game.sh" start