/**
 * Basic functions running on the page load time.
 */

window.onload = async () => {
	try {
		loadHelp(help);
	}
	catch {	}
	try {
		await startup();
	}
	catch {	}
}