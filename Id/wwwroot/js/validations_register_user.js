const startup = async () => {
	let translation = await localize(description);
	document.getElementById("description").innerHTML = translation;
}