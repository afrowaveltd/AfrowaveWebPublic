let allEmojis = [];
let isGrid = true;

async function loadEmojis() {
    try {
        document.getElementById("loading-indicator").style.display = "inline";
        const response = await fetch("/api/emojis");
        if (!response.ok) throw new Error("Failed to fetch emojis");
        allEmojis = await response.json();
        renderTable(allEmojis);
        renderGrid(allEmojis);
    } catch (err) {
        document.body.innerHTML = `<p style="color:red;">${err.message}</p>`;
    } finally {
        document.getElementById("loading-indicator").style.display = "none";
    }
}

function renderTable(data) {
    const tbody = document.querySelector("#emoji-table tbody");
    tbody.innerHTML = "";
    data.forEach(e => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td class="emoji">${e.Utf8String}</td>
            <td>${e.Name}</td>
            <td>${e.UnicodeHex}</td>
            <td><code>${e.CSharpString}</code></td>
            <td>
              <button class="copy" onclick="copyToClipboard('${e.Utf8String}')">Copy emoji</button>
              <button class="copy" onclick="copyToClipboard('${e.Name}')">Copy name</button>
              <button class="copy" onclick="copyToClipboard('${e.CSharpString}')">Copy C#</button>
            </td>
          `;
        tbody.appendChild(row);
    });
}

function renderGrid(data) {
    const grid = document.getElementById("emoji-grid");
    grid.innerHTML = "";
    const format = document.getElementById("format").value;
    const size = document.getElementById("size").value;
    data.forEach(e => {
        const cell = document.createElement("div");
        cell.className = `emoji-cell ${size}`;
        cell.title = e.Name;
        cell.onclick = () => copyToClipboard(e[format]);

        if (size === "big") {
            cell.innerHTML = `<div>${e.Utf8String}</div><div class="label">${e.Name}</div>`;
        } else {
            cell.innerText = e.Utf8String;
        }

        grid.appendChild(cell);
    });
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text)
        .then(() => alert(`Copied: ${text}`))
        .catch(() => alert("Failed to copy."));
}

function toggleView() {
    isGrid = !isGrid;
    document.getElementById("emoji-grid").style.display = isGrid ? "flex" : "none";
    document.getElementById("emoji-table").style.display = isGrid ? "none" : "table";
    document.querySelector("button.toggle").innerText = isGrid ? "Show detailed list" : "Show grid view";
}

function applyTheme() {
    const theme = document.getElementById("theme").value;
    document.body.setAttribute("data-theme", theme);
    localStorage.setItem("preferred-theme", theme);
}

function applyPreferredTheme() {
    const storedTheme = localStorage.getItem("preferred-theme");
    const systemTheme = window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
    const theme = storedTheme || systemTheme;
    document.getElementById("theme").value = theme;
    document.body.setAttribute("data-theme", theme);
}

document.getElementById("search").addEventListener("input", function () {
    const input = this.value.trim().toLowerCase();
    const isExact = input.startsWith("=");
    const query = isExact ? input.substring(1) : input.replace(/\*/g, "");
    const filtered = allEmojis.filter(e => {
        const name = e.Name.toLowerCase();
        if (isExact) return name === query;
        return name.includes(query);
    });
    renderTable(filtered);
    renderGrid(filtered);
});

document.getElementById("format").addEventListener("change", () => renderGrid(allEmojis));
document.getElementById("size").addEventListener("change", () => renderGrid(allEmojis));

loadEmojis();
applyPreferredTheme();