/* =====================================================
   Library Management System - Interactive Features
   ===================================================== */

document.addEventListener('DOMContentLoaded', function() {
  // Initialize all features
  initThemeToggle();
  initReadingQuotes();
  initStaffPicks();
  initSurpriseMe();
  initBookRain();
  initHeartBounce();
  initProgressBar();
  initFunFact();
  initNewsletter();
});

/* =====================================================
   Dark/Light Mode Toggle
   ===================================================== */
function initThemeToggle() {
  const themeToggle = document.getElementById('themeToggle');
  const body = document.body;
  
  // Check for saved theme preference or default to light mode
  const savedTheme = localStorage.getItem('library-theme') || 'light';
  setTheme(savedTheme);
  
  // Toggle theme on button click
  themeToggle.addEventListener('click', function() {
    const currentTheme = body.classList.contains('dark-mode') ? 'dark' : 'light';
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    setTheme(newTheme);
    localStorage.setItem('library-theme', newTheme);
  });
  
  function setTheme(theme) {
    if (theme === 'dark') {
      body.classList.add('dark-mode');
      body.classList.remove('light-mode');
      themeToggle.innerHTML = '<i class="fas fa-sun"></i>';
      themeToggle.setAttribute('aria-label', 'Switch to light mode');
    } else {
      body.classList.add('light-mode');
      body.classList.remove('dark-mode');
      themeToggle.innerHTML = '<i class="fas fa-moon"></i>';
      themeToggle.setAttribute('aria-label', 'Switch to dark mode');
    }
  }
}

/* =====================================================
   Reading Quotes Carousel
   ===================================================== */
function initReadingQuotes() {
  const quotes = [
    {
      text: "A reader lives a thousand lives before he dies. The man who never reads lives only one.",
      author: "George R.R. Martin"
    },
    {
      text: "The only thing that you absolutely have to know, is the location of the library.",
      author: "Albert Einstein"
    },
    {
      text: "I have always imagined that Paradise will be a kind of library.",
      author: "Jorge Luis Borges"
    },
    {
      text: "A library is not a luxury but one of the necessities of life.",
      author: "Henry Ward Beecher"
    },
    {
      text: "The only thing that you absolutely have to know, is the location of the library.",
      author: "Albert Einstein"
    },
    {
      text: "I have always imagined that Paradise will be a kind of library.",
      author: "Jorge Luis Borges"
    },
    {
      text: "A library is not a luxury but one of the necessities of life.",
      author: "Henry Ward Beecher"
    },
    {
      text: "There is no friend as loyal as a book.",
      author: "Ernest Hemingway"
    },
    {
      text: "Reading is to the mind what exercise is to the body.",
      author: "Joseph Addison"
    },
    {
      text: "The more that you read, the more things you will know. The more that you learn, the more places you'll go.",
      author: "Dr. Seuss"
    }
  ];
  
  const quoteText = document.getElementById('quoteText');
  const quoteAuthor = document.getElementById('quoteAuthor');
  let currentQuote = 0;
  
  // Only initialize if elements exist (on homepage)
  if (!quoteText || !quoteAuthor) return;
  
  // Change quote every 8 seconds
  setInterval(() => {
    currentQuote = (currentQuote + 1) % quotes.length;
    updateQuote();
  }, 8000);
  
  function updateQuote() {
    // Fade out
    quoteText.style.opacity = '0';
    quoteAuthor.style.opacity = '0';
    
    setTimeout(() => {
      quoteText.textContent = quotes[currentQuote].text;
      quoteAuthor.textContent = '- ' + quotes[currentQuote].author;
      
      // Fade in
      quoteText.style.opacity = '1';
      quoteAuthor.style.opacity = '1';
    }, 500);
  }
}

/* =====================================================
   Staff Picks Carousel
   ===================================================== */
function initStaffPicks() {
  const carousel = document.getElementById('staffPicksCarousel');
  const prevBtn = document.getElementById('prevPicks');
  const nextBtn = document.getElementById('nextPicks');
  const dotsContainer = document.getElementById('carouselDots');
  
  // Sample staff picks data (in real app, this would come from API)
  const staffPicks = [
    {
      id: 1,
      title: "The Midnight Library",
      author: "Matt Haig",
      cover: "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=300&h=400&fit=crop",
      rating: 4.5
    },
    {
      id: 2,
      title: "Atomic Habits",
      author: "James Clear",
      cover: "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=300&h=400&fit=crop",
      rating: 4.8
    },
    {
      id: 3,
      title: "Project Hail Mary",
      author: "Andy Weir",
      cover: "https://images.unsplash.com/photo-1532012197267-da84d127e765?w=300&h=400&fit=crop",
      rating: 4.7
    },
    {
      id: 4,
      title: "The Silent Patient",
      author: "Alex Michaelides",
      cover: "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=300&h=400&fit=crop",
      rating: 4.4
    },
    {
      id: 5,
      title: "Educated",
      author: "Tara Westover",
      cover: "https://images.unsplash.com/photo-1497633762265-9d179a990aa6?w=300&h=400&fit=crop",
      rating: 4.6
    },
    {
      id: 6,
      title: "Where the Crawdads Sing",
      author: "Delia Owens",
      cover: "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?w=300&h=400&fit=crop",
      rating: 4.5
    }
  ];
  
  let currentIndex = 0;
  let itemsPerView = 3;
  let autoSlideInterval;
  let isAutoSliding = true;
  
  // Only initialize if carousel exists
  if (!carousel) return;
  
  // Calculate items per view based on screen size
  function updateItemsPerView() {
    const width = window.innerWidth;
    if (width < 768) {
      itemsPerView = 1;
    } else if (width < 992) {
      itemsPerView = 2;
    } else {
      itemsPerView = 3;
    }
    updateCarousel();
  }
  
  // Create carousel items
  function createCarouselItems() {
    carousel.innerHTML = '';
    
    staffPicks.forEach((pick, index) => {
      const card = document.createElement('div');
      card.className = 'book-card fade-in';
      card.style.animationDelay = `${index * 0.1}s`;
      card.innerHTML = `
        <img src="${pick.cover}" alt="Cover of ${pick.title}" loading="lazy">
        <div class="book-card-content">
          <h3 class="book-card-title">${pick.title}</h3>
          <p class="book-card-author">by ${pick.author}</p>
          <div class="book-card-rating">
            ${'★'.repeat(Math.floor(pick.rating))}${'☆'.repeat(5 - Math.floor(pick.rating))}
            <span class="rating-value">${pick.rating}</span>
          </div>
          <button class="btn btn-primary btn-sm mt-2 add-to-reading-list" data-book-id="${pick.id}">
            <i class="fas fa-heart"></i> Add to List
          </button>
        </div>
      `;
      carousel.appendChild(card);
    });
  }
  
  // Update carousel display
  function updateCarousel() {
    const cards = carousel.querySelectorAll('.book-card');
    const totalItems = cards.length;
    const maxIndex = Math.max(0, totalItems - itemsPerView);
    
    // Ensure currentIndex is within bounds
    currentIndex = Math.min(currentIndex, maxIndex);
    
    // Calculate card width
    const cardWidth = 100 / itemsPerView;
    
    // Update each card's position
    cards.forEach((card, index) => {
      const position = index - currentIndex;
      card.style.transform = `translateX(${position * 100}%)`;
      card.style.opacity = position >= 0 && position < itemsPerView ? '1' : '0';
      card.style.position = 'absolute';
      card.style.width = `${cardWidth}%`;
      card.style.left = `${currentIndex * cardWidth}%`;
    });
    
    // Update dots
    updateDots();
  }
  
  // Create navigation dots
  function createDots() {
    dotsContainer.innerHTML = '';
    const totalDots = Math.ceil(staffPicks.length / itemsPerView);
    
    for (let i = 0; i < totalDots; i++) {
      const dot = document.createElement('button');
      dot.className = 'carousel-dot';
      dot.setAttribute('aria-label', `Go to slide ${i + 1}`);
      if (i === 0) dot.classList.add('active');
      
      dot.addEventListener('click', () => {
        currentIndex = i * itemsPerView;
        updateCarousel();
        resetAutoSlide();
      });
      
      dotsContainer.appendChild(dot);
    }
  }
  
  // Update active dot
  function updateDots() {
    const dots = dotsContainer.querySelectorAll('.carousel-dot');
    const activeDotIndex = Math.floor(currentIndex / itemsPerView);
    
    dots.forEach((dot, index) => {
      dot.classList.toggle('active', index === activeDotIndex);
    });
  }
  
  // Navigation functions
  function goToPrev() {
    currentIndex = Math.max(0, currentIndex - itemsPerView);
    updateCarousel();
    resetAutoSlide();
  }
  
  function goToNext() {
    const maxIndex = staffPicks.length - itemsPerView;
    currentIndex = Math.min(maxIndex, currentIndex + itemsPerView);
    updateCarousel();
    resetAutoSlide();
  }
  
  // Auto-slide functionality
  function startAutoSlide() {
    if (!isAutoSliding) return;
    autoSlideInterval = setInterval(() => {
      const maxIndex = staffPicks.length - itemsPerView;
      if (currentIndex >= maxIndex) {
        currentIndex = 0;
      } else {
        currentIndex++;
      }
      updateCarousel();
    }, 5000);
  }
  
  function stopAutoSlide() {
    clearInterval(autoSlideInterval);
  }
  
  function resetAutoSlide() {
    stopAutoSlide();
    startAutoSlide();
  }
  
  // Event listeners
  prevBtn.addEventListener('click', goToPrev);
  nextBtn.addEventListener('click', goToNext);
  
  // Pause on hover
  carousel.addEventListener('mouseenter', stopAutoSlide);
  carousel.addEventListener('mouseleave', startAutoSlide);
  
  // Handle window resize
  window.addEventListener('resize', () => {
    updateItemsPerView();
    createDots();
  });
  
  // Initialize
  createCarouselItems();
  updateItemsPerView();
  createDots();
  startAutoSlide();
  
  // Add to reading list functionality
  carousel.addEventListener('click', function(e) {
    if (e.target.closest('.add-to-reading-list')) {
      const button = e.target.closest('.add-to-reading-list');
      const bookId = button.dataset.bookId;
      const icon = button.querySelector('i');
      
      // Toggle heart animation
      icon.classList.add('heart-bounce');
      setTimeout(() => {
        icon.classList.remove('heart-bounce');
      }, 600);
      
      // Change button state
      if (button.classList.contains('added')) {
        button.classList.remove('added');
        button.innerHTML = '<i class="fas fa-heart"></i> Add to List';
        // In real app, make API call to remove from reading list
      } else {
        button.classList.add('added');
        button.innerHTML = '<i class="fas fa-heart"></i> Added!';
        // In real app, make API call to add to reading list
      }
    }
  });
}

/* =====================================================
   Surprise Me Button
   ===================================================== */
function initSurpriseMe() {
  const surpriseBtn = document.getElementById('surpriseMeBtn');
  
  if (!surpriseBtn) return;
  
  surpriseBtn.addEventListener('click', function() {
    // Add click animation
    this.style.transform = 'scale(0.95)';
    setTimeout(() => {
      this.style.transform = '';
    }, 150);
    
    // In a real app, this would fetch a random book from API
    // For now, we'll simulate with a redirect to search with random term
    const randomTerms = ['adventure', 'mystery', 'romance', 'science', 'history', 'fantasy', 'biography'];
    const randomTerm = randomTerms[Math.floor(Math.random() * randomTerms.length)];
    
    // Show loading state
    const originalText = this.innerHTML;
    this.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Finding...';
    
    // Simulate API call delay
    setTimeout(() => {
      // Redirect to books page with search
      window.location.href = `/Books?searchString=${randomTerm}`;
    }, 800);
  });
}

/* =====================================================
   Book Rain Particle Effect
   ===================================================== */
function initBookRain() {
  // Create toggle button for book rain
  const toggleButton = document.createElement('button');
  toggleButton.className = 'btn btn-theme-toggle book-rain-toggle';
  toggleButton.innerHTML = '<i class="fas fa-book"></i>';
  toggleButton.title = 'Toggle book rain effect';
  toggleButton.style.position = 'fixed';
  toggleButton.style.bottom = '20px';
  toggleButton.style.right = '20px';
  toggleButton.style.zIndex = '1000';
  toggleButton.style.display = 'none'; // Hidden by default
  
  document.body.appendChild(toggleButton);
  
  let isRainActive = false;
  let rainInterval;
  const bookEmojis = ['📚', '📖', '📕', '📗', '📘', '📙'];
  
  // Check if on search page (where results would be)
  const isSearchPage = window.location.pathname.includes('/Books') && 
                      window.location.search.includes('searchString');
  
  if (isSearchPage) {
    toggleButton.style.display = 'flex';
  }
  
  toggleButton.addEventListener('click', function() {
    isRainActive = !isRainActive;
    
    if (isRainActive) {
      this.classList.add('active');
      startBookRain();
    } else {
      this.classList.remove('active');
      stopBookRain();
    }
  });
  
  function startBookRain() {
    const container = document.createElement('div');
    container.className = 'book-rain-container';
    container.id = 'bookRainContainer';
    document.body.appendChild(container);
    
    rainInterval = setInterval(() => {
      createFallingBook(container);
    }, 200);
  }
  
  function stopBookRain() {
    clearInterval(rainInterval);
    const container = document.getElementById('bookRainContainer');
    if (container) {
      container.remove();
    }
  }
  
  function createFallingBook(container) {
    const book = document.createElement('div');
    book.className = 'book-rain';
    book.textContent = bookEmojis[Math.floor(Math.random() * bookEmojis.length)];
    
    // Random properties
    const startPosition = Math.random() * window.innerWidth;
    const duration = 3 + Math.random() * 2; // 3-5 seconds
    const size = 20 + Math.random() * 20; // 20-40px
    
    book.style.left = `${startPosition}px`;
    book.style.fontSize = `${size}px`;
    book.style.animationDuration = `${duration}s`;
    
    container.appendChild(book);
    
    // Remove after animation
    setTimeout(() => {
      book.remove();
    }, duration * 1000);
  }
}

/* =====================================================
   Heart Bounce Animation
   ===================================================== */
function initHeartBounce() {
  // This is handled within the staff picks carousel
  // Additional event listeners for other heart buttons
  document.addEventListener('click', function(e) {
    if (e.target.closest('.add-to-reading-list') || 
        e.target.closest('[data-action="add-to-reading-list"]')) {
      const button = e.target.closest('button');
      const icon = button.querySelector('i');
      
      if (icon) {
        icon.classList.add('heart-bounce');
        setTimeout(() => {
          icon.classList.remove('heart-bounce');
        }, 600);
      }
    }
  });
}

/* =====================================================
   Progress Bar Animation
   ===================================================== */
function initProgressBar() {
  // This would be called when user visits their profile/account page
  const progressBars = document.querySelectorAll('.books-read-progress');
  
  progressBars.forEach(bar => {
    const targetWidth = bar.dataset.progress || 0;
    bar.style.setProperty('--progress-width', `${targetWidth}%`);
    bar.classList.add('progress-bar-animated');
  });
}

/* =====================================================
   Fun Fact (Daily)
   ===================================================== */
function initFunFact() {
  const funFactElement = document.getElementById('funFact');
  
  if (!funFactElement) return;
  
  const funFacts = [
    "The library at Alexandria was one of the largest and most significant libraries of the ancient world.",
    "The Dewey Decimal System, used by many libraries, was created by Melvil Dewey in 1876.",
    "The world's largest library is the Library of Congress in Washington, D.C., with over 170 million items.",
    "The first public library in the United States was established in 1833 in Peterborough, New Hampshire.",
    "The word 'library' comes from the Latin word 'liber', which means 'book'.",
    "The oldest known library was found in the ruins of ancient Nineveh, dating back to the 7th century BC.",
    "The Library of Congress has over 838 miles of bookshelves.",
    "The average public library lends over 1 million items per year.",
    "There are more public libraries than McDonald's restaurants in the United States.",
    "The smallest library in the world is the Little Free Library, a global movement of book-sharing boxes."
  ];
  
  // Get today's date to show consistent fact for the day
  const today = new Date();
  const dayOfYear = Math.floor((today - new Date(today.getFullYear(), 0, 0)) / (1000 * 60 * 60 * 24));
  const factIndex = dayOfYear % funFacts.length;
  
  funFactElement.innerHTML = `<p>${funFacts[factIndex]}</p>`;
}

function initNewsletter() {
  const form = document.getElementById('newsletterForm');
  const messageDiv = document.getElementById('newsletterMessage');
  
  if (!form || !messageDiv) return;
  
  form.addEventListener('submit', function(e) {
    e.preventDefault();
    const email = form.querySelector('input[type="email"]').value;
    
    // Simulate API call
    messageDiv.textContent = 'Subscribing...';
    messageDiv.style.color = 'var(--gold-light)';
    
    setTimeout(() => {
      // Success message
      messageDiv.textContent = 'Thank you for subscribing! Check your email for confirmation.';
      messageDiv.style.color = 'var(--success)';
      form.reset();
      
      // Clear message after 5 seconds
      setTimeout(() => {
        messageDiv.textContent = '';
      }, 5000);
    }, 1000);
  });
}

/* =====================================================
   Utility Functions
   ===================================================== */
function debounce(func, wait) {
  let timeout;
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout);
      func(...args);
    };
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
  };
}

// Export functions for potential external use
window.LibraryApp = {
  initThemeToggle,
  initReadingQuotes,
  initStaffPicks,
  initSurpriseMe,
  initBookRain,
  initHeartBounce,
  initProgressBar,
  initFunFact,
  initNewsletter
};