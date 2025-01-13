const policyErr = document.getElementById('policy_mode_err');
const policyMode = document.getElementById('policyMode');
const addOriginInput = document.getElementById('addOriginAddress');

document.addEventListener("DOMContentLoaded", () => {
	const selectElement = document.getElementById("allowedOrigins");
	selectElement.addEventListener("change", function () {
		const selectedValues = Array.from(selectElement.selectedOptions).map(option => option.value);
		Array.from(selectElement.options).forEach(option => {
			if (!selectedValues.includes(option.value)) {
				option.remove();
			}
		});
	});
});

const addOriginToList = (value) => {
	const selectElement = document.getElementById("allowedOrigins");

	// Check if the value is empty or already exists
	if (!value.trim()) return;
	if (Array.from(selectElement.options).some(option => option.value === value)) return;

	// Create new <option> element
	const newOption = document.createElement("option");
	newOption.value = value;
	newOption.textContent = value;
	newOption.selected = true; // Mark it as selected

	// Append new option to the select list
	selectElement.appendChild(newOption);
}

const addOrigin = () => {
	addOriginInput.style.display = 'block';
}