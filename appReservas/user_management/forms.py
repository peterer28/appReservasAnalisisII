from django import forms
from django.contrib.auth.forms import UserCreationForm
from user_management.models import CustomUser
from user_management.models import CustomUserManager


class UserRegistrationForm(UserCreationForm):
    nombre = forms.CharField(max_length=30)
    aMaterno = forms.CharField(max_length=30)
    aPaterno = forms.CharField(max_length=30)
    email = forms.EmailField(max_length=30)
    phone = forms.CharField(max_length=15)
    tipoUsuario = forms.CharField(max_length=20)

class CustomUserRegistrationForm(UserCreationForm):
    class Meta:
        model = CustomUser
        fields = ('nombre', 'aMaterno', 'aPaterno', 'email', 'phone', 'usuario', 'password', 'tipoUsuario')

    def save(self, commit=True):
        user = super(UserRegistrationForm, self).save(commit=False)
        user.set_password(self.cleaned_data["password"])
        if commit:
            user.save()
        return user