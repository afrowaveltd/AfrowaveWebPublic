// variables to get form parts

const policyErr = document.getElementById('policy_mode_err');
const policyMode = document.getElementById('policyMode');
const addOriginButton = document.getElementById('addOriginButton')
const addOriginInput = document.getElementById('addOriginAddress');
const addOriginErr = document.getElementById('add_origin_err');
const allowedOrigins = document.getElementById('allowedOrigins');
const allowedOriginsSelect = document.getElementById('allowedOriginsSelect')
const allowAnyMethod = document.getElementById('allowAnyMethod');
const allowedMethodsSelect = document.getElementById('allowedMethodsSelect');
const allowedMethodsError = document.getElementById('allowed_methods_err');
const allMethodsError = document.getElementById('all_methods_err');
const allowedMethods = document.getElementById('allowedMethods');
const allowAnyHeaders = document.getElementById('allowAnyHeaders');
const allowAnyHeadersError = document.getElementById('allow_any_headers_err');
const allowedHeadersSelect = document.getElementById('allowedHeadersSelect');
const allowCredentials = document.getElementById('allowCredentials');
const submitButton = document.getElementById('submit');

const setValid = (element, errorElement) => {
	element.classList.remove('input-invalid');
	element.classList.add('input-valid');
	errorElement.innerHTML = '';
}

const setInvalid = (element, errorElement, message) => {
	element.classList.remove('input-valid');
	element.classList.add('input-invalid');
	errorElement.innerHTML = message;
}

const isMultiSelectSelected = () => {
	if (!allowedMethods) {
		console.error("Element not found:", selectId);
		return false;
	}

	return [...allowedMethods.options].some(option => option.selected);
}

const checkPolicyMode = async () => {
	if (policyMode.value == 'AllowAll') {
		allowedOriginsSelect.style.display = 'none';
		addOriginButton.style.display = 'none';
		setInvalid(policyMode, policyErr, await localize('This option is less secure'))
	}
	else if (policyMode.value == 'DenyAll') {
		allowedOriginsSelect.style.display = 'none';
		addOriginButton.style.display = 'none';
		setInvalid(policyMode, policyErr, await localize('This will block any cross domain requests'));
	}
	else {
		addOriginButton.style.display = 'inline';
		allowedOriginsSelect.style.display = 'block';
		setValid(policyMode, policyErr);
	}
}

const checkMethod = async () => {
	if (allowAnyMethod.value == 'false' && !isMultiSelectSelected()) {
		setInvalid(allowedMethods, allowedMethodsError, await localize('If you don\'t select any method,all cross domain requests will be blocked'))
	}
	else {
		setValid(allowedMethods, allowedMethodsError)
	}
}

const checkAllMethods = async () => {
	setValid(allowAnyMethod, allMethodsError);
	if (allowAnyMethod.value == 'true') {
		allowedMethodsSelect.style.display = 'none';
	}
	else {
		allowedMethodsSelect.style.display = 'block';
	}
}

const checkAllHeaders = async () => {
	setValid(allowAnyHeaders, allowAnyHeadersError);
	if (allowAnyHeaders.value == 'true') {
		allowedHeadersSelect.style.display = 'none';
	}
	else {
		allowedHeadersSelect.style.display = 'block';
	}
}
const checkAllowedHeaders = async () => {
}

const checkForm = async () => {
	await checkPolicyMode();
	await checkMethod();
	await checkAllMethods();
	await checkAllHeaders();
	await checkAllowedHeaders();
}

const isValidUrl = (url) => {
	try {
		const parsedUrl = new URL(url);
		if (!["http:", "https:"].includes(parsedUrl.protocol)) {
			return false;
		}

		if (!parsedUrl.hostname) {
			return false;
		}

		if (parsedUrl.pathname !== "/" || parsedUrl.search || parsedUrl.hash) {
			return false;
		}
		return true;
	} catch (error) {
		return false;
	}
}

document.addEventListener("DOMContentLoaded", () => {
	allowedOrigins.addEventListener("change", function () {
		const selectedValues = Array.from(allowedOrigins.selectedOptions).map(option => option.value);
		Array.from(allowedOrigins.options).forEach(option => {
			if (!selectedValues.includes(option.value)) {
				option.remove();
			}
		});
	});
});

document.addEventListener("DOMContentLoaded", () => {
	const inputElement = document.getElementById("addOriginValue");

	if (inputElement) {
		inputElement.addEventListener("keydown", function (event) {
			if (event.key === "Enter") {
				event.preventDefault(); // Prevent form submission

				// Get all focusable form elements
				const formElements = [...document.querySelectorAll("input, select, textarea, button")];
				const currentIndex = formElements.indexOf(event.target);

				if (currentIndex !== -1 && currentIndex < formElements.length - 1) {
					formElements[currentIndex + 1].focus(); // Move focus to the next element
				}
			}
		});
	}
});

const addOriginToList = async (value) => {
	if (isValidUrl(value)) {
		if (!value.trim()) return;
		if (Array.from(allowedOrigins.options).some(option => option.value === value)) return;

		const newOption = document.createElement("option");
		newOption.value = value;
		newOption.textContent = value;
		newOption.selected = true;
		allowedOrigins.appendChild(newOption);
		addOriginInput.style.display = 'none';
	}
	else {
		console.log('ok');
		setInvalid(addOriginInput, addOriginErr, await localize("This is not a valid URL"));
	}
}

const addOrigin = () => {
	addOriginInput.style.display = 'block';
}

const startup = async () => checkForm();