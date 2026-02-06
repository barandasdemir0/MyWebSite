
document.addEventListener('DOMContentLoaded', function () {
    const editor = document.getElementById('editorContent');

    if (!editor) return;

    // Track selected media element - use global for easy access
    window.selectedMediaElement = null;

    // Clear selection
    function clearMediaSelection() {
        if (window.selectedMediaElement) {
            window.selectedMediaElement.classList.remove('media-selected');
            window.selectedMediaElement = null;
        }
    }

    // Set media selection
    function selectMedia(element) {
        clearMediaSelection();
        window.selectedMediaElement = element;
        element.classList.add('media-selected');
    }

    // Handle mousedown on media
    editor.addEventListener('mousedown', function (e) {
        let target = e.target;
        let mediaElement = null;

        if (target.tagName === 'IMG' || target.tagName === 'VIDEO' || target.tagName === 'IFRAME') {
            mediaElement = target;
        } else {
            const childMedia = target.querySelector('iframe, video, img');
            if (childMedia) {
                mediaElement = childMedia;
            } else {
                const wrapper = target.closest('.media-wrapper');
                if (wrapper) {
                    mediaElement = wrapper.querySelector('iframe, video, img');
                }
            }
        }

        if (mediaElement) {
            e.preventDefault();
            e.stopPropagation();
            selectMedia(mediaElement);
        } else {
            clearMediaSelection();
        }
    }, true);

    // Expose for global access
    window.getSelectedMedia = function () {
        return window.selectedMediaElement;
    };

    // Word count and reading time update
    function updateStats() {
        const rawText = editor.textContent || '';
        const text = rawText.replace(/\u00A0/g, ' ').trim();
        const wordCount = text ? text.split(/\s+/).length : 0;
        const readTime = wordCount === 0 ? 0 : Math.ceil(wordCount / 200);

        const wordCountEl = document.getElementById('wordCount');
        const readTimeEl = document.getElementById('readTime');
        const readTimeInput = document.getElementById('readTimeInput');

        if (wordCountEl) wordCountEl.textContent = wordCount;
        if (readTimeEl) readTimeEl.textContent = readTime;
        if (readTimeInput) readTimeInput.value = readTime;
    }

    // Color picker handlers
    const textColorPicker = document.getElementById('textColorPicker');
    const bgColorPicker = document.getElementById('bgColorPicker');
    const textColorIndicator = document.getElementById('textColorIndicator');

    if (textColorPicker) {
        textColorPicker.addEventListener('change', function () {
            editor.focus();
            document.execCommand('foreColor', false, this.value);
            if (textColorIndicator) textColorIndicator.style.background = this.value;
        });
    }

    if (bgColorPicker) {
        bgColorPicker.addEventListener('change', function () {
            editor.focus();
            document.execCommand('hiliteColor', false, this.value);
        });
    }

    // Update word count on content change
    editor.addEventListener('input', function () {
        updateStats();

        // Auto-highlight code blocks
        if (typeof hljs !== 'undefined') {
            editor.querySelectorAll('pre code').forEach((block) => {
                block.className = '';
                hljs.highlightElement(block);
            });
        }
    });

    // Keyboard shortcuts
    editor.addEventListener('keydown', function (e) {
        if (e.ctrlKey || e.metaKey) {
            switch (e.key.toLowerCase()) {
                case 'b': e.preventDefault(); document.execCommand('bold'); break;
                case 'i': e.preventDefault(); document.execCommand('italic'); break;
                case 'u': e.preventDefault(); document.execCommand('underline'); break;
            }
        }
    });

    // Initial stats update
    updateStats();

    // === UPDATE SAYFASI İÇİN ===
    // 1. Content
    const hiddenContent = document.getElementById('hiddenContentInput');
    if (hiddenContent && hiddenContent.value.trim()) {
        editor.innerHTML = hiddenContent.value;
        updateStats();
    }

    // 2. Technologies
    const hiddenTags = document.getElementById('hiddenTagsInput');
    const tagsContainer = document.querySelector('.tags-input');
    const tagsInputField = document.getElementById('tagsInput');
    if (hiddenTags && hiddenTags.value && tagsContainer && tagsInputField) {
        hiddenTags.value.split(',').filter(t => t.trim()).forEach(tag => {
            const tagEl = document.createElement('span');
            tagEl.className = 'tag-item';
            tagEl.dataset.value = tag.trim();
            tagEl.innerHTML = `<span>${tag.trim()}</span><button type="button">×</button>`;
            tagEl.querySelector('button').addEventListener('click', () => tagEl.remove());
            tagsContainer.insertBefore(tagEl, tagsInputField);
        });
    }

    // 3. CoverImage
    const hiddenImage = document.getElementById('featuredImageInput');
    const imageContainer = document.querySelector('.featured-image-upload');
    if (hiddenImage && hiddenImage.value && imageContainer) {
        imageContainer.innerHTML = `<img src="${hiddenImage.value}" style="width:100%; height:100%; object-fit:cover; border-radius:8px;">`;
        imageContainer.style.height = '180px';
    }

    // Clear editor
    const clearBtn = document.querySelector('[data-action="clearEditor"]');
    if (clearBtn) {
        clearBtn.addEventListener('click', function () {
            editor.innerHTML = '';
            clearMediaSelection();
            updateStats();
        });
    }

    // Font size select handler
    const fontSizeSelect = document.getElementById('fontSizeSelect');
    if (fontSizeSelect) {
        fontSizeSelect.addEventListener('change', function () {
            editor.focus();
            document.execCommand('fontSize', false, this.value);
            if (typeof currentFontSize !== 'undefined') {
                currentFontSize = parseInt(this.value);
            }
        });
    }

    // Tags input handler
    const tagsInput = document.getElementById('tagsInput');
    if (tagsInput) {
        tagsInput.addEventListener('keydown', function (event) {
            if (typeof addTag === 'function') {
                addTag(event);
            }
        });
    }

    // Form Submit Handler
    const form = document.getElementById('createPostForm');
    if (form) {
        form.addEventListener('submit', function () {
            // ✅ ReadTime'ı güncelle
            updateStats();

            // 1. İçeriği (HTML) al, gizli textarea'ya bas
            const editorContent = document.getElementById('editorContent');
            const hiddenInput = document.getElementById('hiddenContentInput');

            if (editorContent && hiddenInput) {
                hiddenInput.value = editorContent.innerHTML;
            }

            // 2. Etiketleri (Tags) al, gizli inputa bas
            const tagSpans = document.querySelectorAll('.tag-item');
            const hiddenTagsEl = document.getElementById('hiddenTagsInput');
            if (hiddenTagsEl) {
                const tagsArray = Array.from(tagSpans).map(span => span.dataset.value);
                hiddenTagsEl.value = tagsArray.join(',');
            }
        });
    }

    // ✅ SELECT2 KATEGORİ - Gecikme ile timing sorununu çözer
    setTimeout(function () {
        if (typeof $ !== 'undefined' && $('#postCategory').length > 0) {
            // Eğer daha önce başlatıldıysa yok et
            if ($('#postCategory').hasClass('select2-hidden-accessible')) {
                $('#postCategory').select2('destroy');
            }

            var categorySelect = $('#postCategory').select2({
                placeholder: "Kategori seçin (Çoklu)",
                allowClear: true,
                width: '100%'
            });

            // Seçili değerleri zorla göster
            var selectedVals = [];
            $('#postCategory option[selected]').each(function () {
                selectedVals.push($(this).val());
            });
            if (selectedVals.length > 0) {
                categorySelect.val(selectedVals).trigger('change');
            }
        }
    }, 100);

});

// Add tag to the tag list on Enter
function addTag(event) {
    if (event.key !== 'Enter') return;

    event.preventDefault();
    const input = event.target;
    const value = input.value.trim();
    if (!value) return;

    const container = input.closest('.tags-input');
    if (!container) return;

    // Skip duplicates (case-insensitive)
    const exists = Array.from(container.querySelectorAll('.tag-item'))
        .some(tag => (tag.dataset.value || '').toLowerCase() === value.toLowerCase());
    if (exists) {
        input.value = '';
        return;
    }

    const tagEl = document.createElement('span');
    tagEl.className = 'tag-item';
    tagEl.dataset.value = value;

    const label = document.createElement('span');
    label.textContent = value;

    const removeBtn = document.createElement('button');
    removeBtn.type = 'button';
    removeBtn.setAttribute('aria-label', 'Etiketi kaldır');
    removeBtn.textContent = '×';
    removeBtn.addEventListener('click', function () {
        tagEl.remove();
    });

    tagEl.append(label, removeBtn);
    container.insertBefore(tagEl, input);
    input.value = '';
}
