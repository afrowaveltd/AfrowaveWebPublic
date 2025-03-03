// References to key form elements and error containers for the CORS policy configuration form.
const policyErr = document.getElementById('policy_mode_err');
const policyMode = document.getElementById('policyMode');
const addOriginButton = document.getElementById('addOriginButton');
const addOriginInput = document.getElementById('addOriginAddress');
const addOriginErr = document.getElementById('add_origin_err');
const allowedOrigins = document.getElementById('allowedOrigins');
const allowedOriginsSelect = document.getElementById('allowedOriginsSelect');
const allowAnyMethod = document.getElementById('allowAnyMethod');
const allowedMethodsSelect = document.getElementById('allowedMethodsSelect');
const allowedMethodsError = document.getElementById('allowed_methods_err');
const allMethodsError = document.getElementById('all_methods_err');
const allowedMethods = document.getElementById('allowedMethods');
const allowAnyHeaders = document.getElementById('allowAnyHeaders');
const allowAnyHeadersError = document.getElementById('allow_any_headers_err');
const allowedHeadersSelect = document.getElementById('allowedHeadersSelect');
const allowCredentials = document.getElementById('allowCredentials');
const allowCredentialsError = document.getElementById('allow_credentials_err');
const submitButton = document.getElementById('submit');

/**
 * Helper to mark a field as valid.
 */
const setValid = (element, errorElement) => {
	element.classList.remove('input-invalid');
	element.classList.add('input-valid');
	errorElement.innerHTML = '';
}

/**
 * Helper to mark a field as invalid.
 */
const setInvalid = (element, errorElement, message) => {
	element.classList.remove('input-valid');
	element.classList.add('input-invalid');
	errorElement.innerHTML = message;
}

/**
 * Check if at least one method is selected in multi-select.
 */
const isMultiSelectSelected = () => {
	return allowedMethods && [...allowedMethods.options].some(option => option.selected);
}

/**
 * Validate the selected policy mode and update UI accordingly.
 */
const checkPolicyMode = async () => {
	if (policyMode.value === 'AllowAll') {
		allowedOriginsSelect.style.display = 'none';
		addOriginButton.style.display = 'none';
		setInvalid(policyMode, policyErr, await localize('This option is less secure'));
	} else if (policyMode.value === 'DenyAll') {
		allowedOriginsSelect.style.display = 'none';
		addOriginButton.style.display = 'none';
		setInvalid(policyMode, policyErr, await localize('This will block any cross domain requests'));
	} else {
		addOriginButton.style.display = 'inline';
		allowedOriginsSelect.style.display = 'block';
		setValid(policyMode, policyErr);
	}
}

/**
 * Validate selected methods if "Allow Any Method" is set to false.
 */
const checkMethod = async () => {
	if (allowAnyMethod.value === 'false' && !isMultiSelectSelected()) {
		setInvalid(allowedMethods, allowedMethodsError, await localize('If you don\'t select any method, all cross domain requests will be blocked'));
	} else {
		setValid(allowedMethods, allowedMethodsError);
	}
}

/**
 * Toggle visibility of allowed methods select based on "Allow Any Method".
 */
const checkAllMethods = async () => {
	setValid(allowAnyMethod, allMethodsError);

	allowedMethodsSelect.style.display = allowAnyMethod.value === 'true' ? 'none' : 'block';
}

/**
 * Toggle visibility of allowed headers select based on "Allow Any Headers".
 */
const checkAllHeaders = async () => {
	setValid(allowAnyHeaders, allowAnyHeadersError);

	allowedHeadersSelect.style.display = allowAnyHeaders.value === 'true' ? 'none' : 'block';
}

/**
 * Placeholder for custom header validation (future use).
 */
const checkAllowedHeaders = async () => {
	// Currently no validation required here.
}

/**
 * Validate if "Allow Credentials" is allowed under the current policy mode.
 */
const checkAllowCredentials = async () => {
	if (policyMode.value === 'AllowAll' && allowCredentials.value === 'true') {
		allowCredentials.value = 'false';
		setInvalid(allowCredentials, allowCredentialsError, await localize('Policy \'allow all\' can\'t be combined with allowing credentials'));
	} else {
		setValid(allowCredentials, allowCredentialsError);
	}
}

/**
 * Run all form validation checks in sequence.
 */
const checkForm = async () => {
	await checkPolicyMode();
	await checkMethod();
	await checkAllMethods();
	await checkAllHeaders();
	await checkAllowedHeaders();
	await checkAllowCredentials();
}

/**
 * Validate if the provided URL is a valid HTTP/HTTPS origin.
 */
const isValidUrl = (url) => {
	try {
		const parsedUrl = new URL(url);
		return ["http:", "https:"].includes(parsedUrl.protocol) &&
			parsedUrl.hostname &&
			parsedUrl.pathname === "/" &&
			!parsedUrl.search &&
			!parsedUrl.hash;
	} catch (error) {
		return false;
	}
}

/**
 * Remove unselected origins from the allowed origins list.
 */
document.addEventListener("DOMContentLoaded", () => {
	allowedOrigins.addEventListener("change", () => {
		const selectedValues = Array.from(allowedOrigins.selectedOptions).map(option => option.value);

		Array.from(allowedOrigins.options).forEach(option => {
			if (!selectedValues.includes(option.value)) {
				option.remove();
			}
		});
	});
});

/**
 * Enable "Enter" key to jump to the next input in the form.
 */
document.addEventListener("DOMContentLoaded", () => {
	const inputElement = document.getElementById("addOriginValue");
	if (inputElement) {
		inputElement.addEventListener("keydown", (event) => {
			if (event.key === "Enter") {
				event.preventDefault();

				const formElements = [...document.querySelectorAll("input, select, textarea, button")];
				const currentIndex = formElements.indexOf(event.target);

				if (currentIndex !== -1 && currentIndex < formElements.length - 1) {
					formElements[currentIndex + 1].focus();
				}
			}
		});
	}
});

/**
 * Adds a new origin to the allowed origins list if valid.
 * @param {string} value - The origin URL.
 */
const addOriginToList = async (value) => {
	if (isValidUrl(value)) {
		if (!value.trim() || Array.from(allowedOrigins.options).some(option => option.value === value)) {
			return;
		}

		const newOption = document.createElement("option");
		newOption.value = value;
		newOption.textContent = value;
		newOption.selected = true;

		allowedOrigins.appendChild(newOption);
		addOriginInput.style.display = 'none';
	} else {
		setInvalid(addOriginInput, addOriginErr, await localize("This is not a valid URL"));
	}
}

/**
 * Display the "Add Origin" input field.
 */
const addOrigin = () => {
	addOriginInput.style.display = 'block';
}

/**
 * Perform initial form validation on page load.
 */
const startup = async () => checkForm();