window.focusRow = function (id) {
    const fila = document.getElementById(id);
    if (fila) {
        // Desplaza la vista suavemente hasta la fila correspondiente[cite: 7]
        fila.scrollIntoView({ behavior: "smooth", block: "nearest" });
        fila.focus();
    }
};