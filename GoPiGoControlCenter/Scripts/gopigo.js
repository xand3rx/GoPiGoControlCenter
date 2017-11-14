﻿var isWatcher = false;
var goPiGoHub;

$(document).ready(function() {
	goPiGoHub = $.connection.goPiGoHub;

	if ($("#watcherCommands").length) {
		isWatcher = true;
		goPiGoHub.client.executeCommand = function(data) { goPiGoHub.log(data); };
		goPiGoHub.log = function(message) {
			$("#watcherCommands").append(message + "<br />");
		};
	} else {
		initControlCenter();
	}

	$.connection.hub.disconnected(function() {
		goPiGoHub.log("Disconnected! Reconnecting automatically...");
		startHub(2000);
	});

	if ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected
		&& $.connection.hub.state !== $.signalR.connectionState.connecting) {
		startHub(0);
	}
});

function startHub(timeout) {
	setTimeout(function() {
		if (navigator.onLine) {
			$.connection.hub.start()
				.done(function () {
					goPiGoHub.log("Connected to server!");
					if (!isWatcher) {
						goPiGoHub.server.getCarConnectionStatus()
							.done(setCarConnected);
					}
				});
		} else {
			startHub(5000); // Retry connection after 5 seconds.
		}
	}, timeout);
}

function setCarConnected(isConnected) {
	if (isConnected) {
		$("#carStatus").text("GoPiGo Car Connected!");
	} else {
		$("#carStatus").text("Waiting for car to connect...");
	}
}

function initControlCenter() {
	goPiGoHub.log = function (message) {
		$("#status").text(message);
	};
	goPiGoHub.client.sendCarConnected = function (data) {
		setCarConnected(data);
	};

	goPiGoHub.client.showPicture = function(uri) {
		$("#picture").attr("src", uri);
	}


	$("#btnForward").click(function () {
		goPiGoHub.server.sendCommand("Forward");
	});
	$("#btnRight").click(function () {
		goPiGoHub.server.sendCommand("Right");
	});
	$("#btnBackward").click(function () {
		goPiGoHub.server.sendCommand("Backward");
	});
	$("#btnLeft").click(function () {
		goPiGoHub.server.sendCommand("Left");
	});
	$("#btnStop").click(function () {
		goPiGoHub.server.sendCommand("Stop");
	});
	$("#btnToggleLeds").click(function () {
		goPiGoHub.server.sendCommand("ToggleLeds");
	});
	$("#btnTakePicture").click(function () {
		goPiGoHub.server.sendCommand("TakePicture");
	});
}