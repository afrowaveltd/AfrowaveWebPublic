﻿window.onload = async () => {
	try {
		loadHelp(help);
	}
	catch {
		console.log("No help function");
	}
	try {
		await startup();
	}
	catch {
		console.log("No startup function");
	}
}