https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification

function scrollCarousel(key, direction) {
    const carousel = document.getElementById('carousel-' + key);
    if (carousel) {
        const scrollAmount = 140;
        carousel.scrollBy({ left: direction * scrollAmount, behavior: 'smooth' });
    }
}
