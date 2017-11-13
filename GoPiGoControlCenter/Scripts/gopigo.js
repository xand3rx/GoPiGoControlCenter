var isWatcher = false;
var goPiGoHub;

$(document).ready(function () {
	goPiGoHub = $.connection.goPiGoHub;

	if ($("#watcherCommands").length) {
		isWatcher = true;
		goPiGoHub.client.executeCommand = function (data) { goPiGoHub.log(data); }
		goPiGoHub.log = function (message) {
			$("#watcherCommands").append(message + "<br />");
		}
	} else {
		goPiGoHub.log = function (message) {
			$("#status").text(message);
		}
		$("#btnForward").click(function () {
			goPiGoHub.server.sendCommand("Forward");
		}
		);
		$("#btnRight").click(function () {
			goPiGoHub.server.sendCommand("Right");
		}
		);
		$("#btnBackward").click(function () {
			goPiGoHub.server.sendCommand("Backward");
		}
		);
		$("#btnLeft").click(function () {
			goPiGoHub.server.sendCommand("Left");
		}
		);
		$("#btnStop").click(function () {
			goPiGoHub.server.sendCommand("Stop");
		}
		);
	}

	$.connection.hub.disconnected(function () {
		goPiGoHub.log("Disconnected!");
		startHub(2000);
	});

	if ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected
		&& $.connection.hub.state !== $.signalR.connectionState.connecting) {
		startHub(0);
	}
});

function startHub(timeout) {
	setTimeout(function () {
		if (navigator.onLine) {
			$.connection.hub.start()
				.done(function () {
					goPiGoHub.log("Connected!");
				});
		} else {
			startHub(5000); // Restart connection after 5 seconds.
		}
	}, timeout);
}