#!/bin/bash

CMD=$1

CUR_DIR=$(dirname $(readlink -f $0))
PID_FILE=$CUR_DIR/main.pid

function start(){
	rm -rf $CUR_DIR/log/main.log
	echo "start main service"
	$CUR_DIR/3rd/skynet/skynet $CUR_DIR/config/config.main
}

function stop(){
	if [ ! -f $PID_FILE ] ;then
		exit 0
	fi
	PID=$(cat $PID_FILE)
	PID_EXIST=$(ps aux | awk '{print $2}'| grep -w $PID)
	if [ ! $PID_EXIST ] ;then
		exit 0
	else
		echo "stop main service"
		kill -9 $PID
	fi
}

case "$CMD" in
	start )
		start
		;;
	stop )
		stop
		;;
	*)
	exit 2
esac