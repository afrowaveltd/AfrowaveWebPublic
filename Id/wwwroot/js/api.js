// Purpose: Contains the functions that are used to interact with the API.

// Result class

class Result {
    constructor(success, data = null, error = null) {
        this.success = success; // Boolean: true if request succeeded
        this.data = data;       // Data: server output (JSON, text, etc.)
        this.error = error;     // Error: { code, message } in case of failure
    }
}

// API Request function

const apiRequest = async ({
    url,
    method = 'GET',
    data = null,
    headers = {},
    responseType = 'json',
    useFormData = false,
    token = null,
    formName = null // Optional form name
} = {}) => {
    try {
        // Set default headers if not using FormData
        if (!useFormData && !headers['Content-Type']) {
            headers['Content-Type'] = 'application/json';
        }

        // Add Authorization header if token is provided
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        let options = {
            method,
            headers,
        };

        // Handle FormData when formName is provided
        if (useFormData && formName) {
            const formElement = document.forms[formName];
            if (!formElement) {
                return new Result(false, null, { code: 404, message: `Form with name "${formName}" not found on the page.` });
            }
            const formData = new FormData(formElement);
            options.body = formData;
        }
        // Handle FormData when an HTMLElement is passed as data
        else if (useFormData && data instanceof HTMLElement) {
            const formData = new FormData(data);
            options.body = formData;
        }
        // Handle FormData when an object is passed
        else if (useFormData && data instanceof Object) {
            const formData = new FormData();
            Object.entries(data).forEach(([key, value]) => formData.append(key, value));
            options.body = formData;
        }
        // Handle JSON and URL-encoded data
        else if (data) {
            if (headers['Content-Type'] === 'application/x-www-form-urlencoded') {
                options.body = new URLSearchParams(data).toString();
            } else {
                options.body = JSON.stringify(data);
            }
        }

        // For GET requests, append data as query parameters
        if (method.toUpperCase() === 'GET' && data) {
            const queryParams = new URLSearchParams(data).toString();
            url += `?${queryParams}`;
        }

        // Make the API call
        const response = await fetch(url, options);

        // Check if the response status is OK (200-299)
        if (!response.ok) {
            return new Result(false, null, { code: response.status, message: response.statusText });
        }

        // Parse the response based on the specified type
        let responseData;
        if (responseType === 'json') {
            responseData = await response.json();
        } else if (responseType === 'text' || responseType === 'html') {
            responseData = await response.text();
        } else {
            return new Result(false, null, { code: 500, message: `Unsupported response type: ${responseType}` });
        }

        return new Result(true, responseData, null);
    } catch (error) {
        console.error('API Request Error:', error);
        return new Result(false, null, { code: 500, message: error.message });
    }
};

// Submit Form by Name

const submitForm = async (formName, url, method = 'POST', token = null) => {
    const result = await apiRequest({ url, method, useFormData: true, formName, token });
    return result;
}

// Fetch Data from API

const fetchData = async (url, token = null) => {
    const result = await apiRequest({ url, token });
    return result;
}

// Fetch Text data from API
const fetchText = async (url, token = null) => {
    const result = await apiRequest({ url, token, responseType: 'text' });
    return result;
}

// Fetch HTML data from API
const fetchHTML = async (url, token = null) => {
    const result = await apiRequest({ url, token, responseType: 'html' });
    return result;
}

// All previous fetches with data parameter

const fetchDataWithBody = async (url, data, method = 'POST', token = null) => {
    const result = await apiRequest({ url, method, data, token });
    return result;
}

// Fetch Data with parameters and headers
const fetchDataWithParamsAndHeaders = async (url, params, headers, token = null) => {
    const result = await apiRequest({ url, data: params, headers, token });
    return result;
}

// fetch text with parameters and headers
const fetchTextWithParamsAndHeaders = async (url, params, headers, token = null) => {
    const result = await apiRequest({ url, data: params, headers, token, responseType: 'text' });
    return result;
}
// fetch html with parameters and headers
const fetchHTMLWithParamsAndHeaders = async (url, params, headers, token = null) => {
    const result = await apiRequest({ url, data: params, headers, token, responseType: 'html' });
    return result;
}

// Delete data from API
const deleteData = async (url, token = null) => {
    const result = await apiRequest({ url, method: 'DELETE', token });
    return result;
}

// Update data using API
const updateData = async (url, data, token = null) => {
    const result = await apiRequest({ url, method: 'PUT', data, token });
    return result;
}

// Delete data with parameters and headers
const deleteDataWithParamsAndHeaders = async (url, params, headers, token = null) => {
    const result = await apiRequest({ url, method: 'DELETE', data: params, headers, token });
    return result;
}

// Update data with parameters and headers

const updateDataWithParamsAndHeaders = async (url, data, headers, token = null) => {
    const result = await apiRequest({ url, method: 'PUT', data, headers, token });
    return result;
}

// Post data with parameters and headers
const postDataWithParamsAndHeaders = async (url, data, headers, token = null) => {
    const result = await apiRequest({ url, method: 'POST', data, headers, token });
    return result;
}

// Post data with parameters but no headers
const postDataWithParams = async (url, data, token = null) => {
    const result = await apiRequest({ url, method: 'POST', data, token });
    return result;
}

// Get localized data from API
const localize = async (text, language = "") => {
    const getCookie = (cname) => {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length).trim();
            }
        }
        return "";
    }
    if (language == "")
        language = getCookie("language")
    if (language = "")
        language = getCookie("Language")
    language == "" ? language = "en" : language = language;
    let res;
    try {
        let url = '/api/GetLocalized/' + text + '/' + language;
        console.log(url)
        const result = await fetch('/api/GetLocalized/' + text + '/' + language);
        res = await result.text();
        console.log(res);
    } catch (error) {
        res = text;
    }
    return res;
}

// Display help div
const loadHelp = async (helpTopic) => {
    const slow = "fast";
    console.log(helpTopic)
    const element = document.getElementById('div_help');
    const link = '/Helps/' + helpTopic;
    $(element).hide(slow);

    const result = await fetch(link);
    if (result.status == 404) {
        let translation = await localize("Help topic not found");
        console.log(translation);
        element.innerHTML = translation;
        $(element).show(slow);
        return;
    }
    let txt = await result.text();

    element.innerHTML = txt;

    $(element).show(slow);
}

const checkIfEmailExists = async (email) => {
    const result = await fetchData('/api/CheckIfEmailExists/' + email);
    return result;
}