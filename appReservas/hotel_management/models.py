# hotel_management/models.py
from django.db import models

class Hotel(models.Model):
    nombre = models.CharField(max_length=30)
    direccion = models.CharField(max_length=50)
    email = models.CharField(max_length=30)
    phone = models.CharField(max_length=15)

    def __str__(self):
        return self.nombre

    class Meta:
        app_label = 'hotel_management'
