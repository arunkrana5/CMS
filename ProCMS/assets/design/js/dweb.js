/**!
 * dweb JS
 */

(function ($) {

	'use strict';

	jQuery(document).ready(function() {

		// Comments

		jQuery('.commentlist li').addClass('card');
		jQuery('.comment-reply-link').addClass('btn btn-secondary');

		// Forms

		jQuery('select, input[type=text], input[type=email], input[type=password], textarea').addClass('form-control');
		jQuery('input[type=submit]').addClass('btn btn-primary');

		// Pagination fix for ellipsis

		jQuery('.pagination .dots').addClass('page-link').parent().addClass('disabled');

		// You can put your own code in here

	});

});
