# hotel_management/forms.py
from django import forms
from .models import Hotel

class HotelForm(forms.ModelForm):
    class Meta:
        model = Hotel
        fields = ['nombre', 'direccion', 'email', 'phone']

