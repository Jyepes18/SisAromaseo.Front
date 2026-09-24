window.cerrarModal = (id) => {
    const modalElement = document.getElementById(id);

    if (!modalElement) return;

    const modal = bootstrap.Modal.getInstance(modalElement);

    if (modal) {
        modal.hide();
    }
    
    setTimeout(() => {
        document.querySelectorAll('.modal-backdrop').forEach(backdrop => {
            backdrop.remove();
        });

        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('overflow');
        document.body.style.removeProperty('padding-right');
    }, 300);
};

window.abrirModal = (id) => {
    const modalElement = document.getElementById(id);

    if (!modalElement) return;

    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);

    modal.show();
};

window.confirmarEliminacion = async function () {
    const result = await Swal.fire({
        title: "¿Está seguro?",
        text: "¡No podrá revertir esta acción!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    });

    return result.isConfirmed;
};

window.descargarPdfBase64 = function (base64, nombreArchivo) {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);

    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);

    const blob = new Blob(
        [byteArray],
        { type: "application/pdf" }
    );

    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.download = nombreArchivo;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    URL.revokeObjectURL(url);
};

window.auth = {

    setCookie: function (token) {
        document.cookie =
            "token=" +
            encodeURIComponent(token) +
            "; path=/; max-age=86400; SameSite=Lax";
    },

    getCookie: function () {
        const nombre = "token=";

        const cookies = document.cookie.split(";");

        for (let cookie of cookies) {
            cookie = cookie.trim();

            if (cookie.startsWith(nombre)) {
                return decodeURIComponent(
                    cookie.substring(nombre.length)
                );
            }
        }

        return null;
    },

    removeCookie: function () {
        document.cookie =
            "token=; path=/; max-age=0; SameSite=Lax";
    }

};