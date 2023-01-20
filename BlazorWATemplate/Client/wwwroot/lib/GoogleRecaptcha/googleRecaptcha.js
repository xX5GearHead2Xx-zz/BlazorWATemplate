function googleRecaptcha(dotNetObject, selector, sitekeyValue) {
     return grecaptcha.render(selector, {
         'sitekey': sitekeyValue,
         'callback': (response) => { dotNetObject.invokeMethodAsync('CallbackOnSuccess', response); },
         'expired-callback': () => { dotNetObject.invokeMethodAsync('CallbackOnExpired'); }
     });
 };