https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification

function scrollMovies(btn, direction) {
    const wrapper = btn.closest('.movie-scroll-wrapper');
    const list = wrapper.querySelector('.movie-scroll-list');
    const card = list.querySelector('.movie-scroll-card');
    if (!card) return;
    const scrollAmount = card.offsetWidth + 24;
    list.scrollBy({ left: direction * scrollAmount, behavior: 'smooth' });
}

document.addEventListener("DOMContentLoaded", function () {
    const favBtn = document.getElementById("favoriteBtn");
    if (favBtn) {
        favBtn.addEventListener("click", function () {
            const filmId = this.getAttribute("data-id");
            const isFav = this.getAttribute("data-fav") === "true";
            const url = isFav ? "/Movie/RemoveFavorite" : "/Movie/AddFavorite";
            fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: "filmId=" + filmId
            })
            .then(r => r.json())
            .then(data => {
                if (data.success) {
                    this.setAttribute("data-fav", (!isFav).toString());
                    if (isFav) {
                        this.className = "btn btn-warning fw-bold px-4 py-2";
                        this.innerHTML = '<i class="bi bi-star-fill"></i> Favorilere Ekle';
                    } else {
                        this.className = "btn btn-danger fw-bold px-4 py-2";
                        this.innerHTML = '<i class="bi bi-star"></i> Favorilerden Kaldır';
                    }
                    const msg = document.getElementById("favoriteMessage");
                    msg.textContent = data.message;
                    msg.style.display = "inline";
                    setTimeout(() => { msg.style.display = "none"; }, 2000);
                }
            });
        });
    }

    const themeToggle = document.getElementById("theme-toggle");
    if (themeToggle) {
        const htmlElement = document.documentElement;
        const icon = themeToggle.querySelector("i");
        
        if (htmlElement.classList.contains("dark-mode")) {
            icon.classList.remove("bi-sun");
            icon.classList.add("bi-moon");
        } else {
            icon.classList.remove("bi-moon");
            icon.classList.add("bi-sun");
        }

        themeToggle.addEventListener("click", function () {
            let theme = htmlElement.classList.contains("dark-mode") ? "light" : "dark";
            setTheme(theme);
            localStorage.setItem("theme", theme);
            this.blur();
        });

        function setTheme(mode) {
            if (mode === "light") {
                htmlElement.classList.add("light-mode");
                htmlElement.classList.remove("dark-mode");
                if (icon) {
                    icon.classList.remove("bi-moon");
                    icon.classList.add("bi-sun");
                }
            } else {
                htmlElement.classList.add("dark-mode");
                htmlElement.classList.remove("light-mode");
                if (icon) {
                    icon.classList.remove("bi-sun");
                    icon.classList.add("bi-moon");
                }
            }
        }
    }

});

function showMessage(message) {
    let msgDiv = document.getElementById('fav-message');
        msgDiv = document.createElement('div');
        msgDiv.id = 'fav-message';
        msgDiv.style.position = 'fixed';
        msgDiv.style.top = '20px';
        msgDiv.style.right = '20px';
        msgDiv.style.background = '#333';
        msgDiv.style.color = '#fff';
        msgDiv.style.padding = '10px 20px';
        msgDiv.style.borderRadius = '5px';
        msgDiv.style.zIndex = 9999;
        document.body.appendChild(msgDiv);
    }
    msgDiv.textContent = message;
    msgDiv.style.display = 'block';
    setTimeout(() => { msgDiv.style.display = 'none'; }, 2000);


document.querySelectorAll('.btn').forEach(function(btn) {
    btn.addEventListener('click', function() {
        var filmId = this.getAttribute('data-filmid');
        var isFav = this.getAttribute('data-fav') === 'true';
        var url = isFav ? '/Movie/RemoveFavorite' : '/Movie/AddFavorite';

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: 'filmId=' + filmId
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showMessage(data.message);
                if (isFav) {
                    this.classList.remove('btn-danger');
                    this.classList.add('btn-warning');
                    this.innerHTML = '<i class="bi bi-star-fill"></i> Favorilere Ekle';
                } else {
                    this.classList.remove('btn-warning');
                    this.classList.add('btn-danger');
                    this.innerHTML = '<i class="bi bi-star"></i> Favorilerden Kaldır';
                }
                this.setAttribute('data-fav', (!isFav).toString());
            } else {
                showMessage(data.message);
            }
        });
    });
});


$(document).ready(function () {
    var $input = $('.navbar .form-control[type="search"]');
    var $suggestions = $('#search-suggestions');

    $input.on('input', function () {
        var query = $(this).val();
        if (query.length < 2) {
            $suggestions.empty().hide();
            return;
        }
        $.get('/Movie/SearchSuggestions', { q: query }, function (data) {
            $suggestions.empty();
            if (data && data.length > 0) {
                data.forEach(function (item) {
                    $suggestions.append('<div class="suggestion-item">' + item + '</div>');
                });
                $suggestions.show();
            } else {
                $suggestions.hide();
            }
        });
    });

    $suggestions.on('click', '.suggestion-item', function () {
        $input.val($(this).text());
        $suggestions.empty().hide();
        $input.closest('form').submit();
    });

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.navbar').length) {
            $suggestions.empty().hide();
        }
    });
});