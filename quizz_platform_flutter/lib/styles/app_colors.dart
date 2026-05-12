import 'package:flutter/material.dart';

class AppColors {
  // Główny fiolet (tło i panele)
  static const Color primary = Color(0xFF6A2B6A);
  static const Color primaryLight = Color(0xFF914D91);
  
  // Akcenty (przyciski, ikony, zaznaczenia)
  static const Color accent = Colors.amber;
  static const Color success = Colors.greenAccent;
  static const Color error = Colors.redAccent;

  // Teksty
  static const Color textPrimary = Colors.white;
  static const Color textSecondary = Colors.white70;
  static const Color textMuted = Colors.white54;
  static const Color textAccent = Colors.amber;

  // Inne
  static const Color cardOverlay = Colors.white10; // (white.withOpacity(0.1))
}