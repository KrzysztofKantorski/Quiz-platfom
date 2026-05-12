import 'package:flutter/material.dart';
import 'app_colors.dart';

class AppTextStyles {
  static const TextStyle heading = TextStyle(
    fontSize: 40,
    color: AppColors.accent,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle listTitle = TextStyle(
    fontSize: 22,
    color: AppColors.accent,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle cardTitle = TextStyle(
    color: AppColors.textPrimary,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle cardSubtitle = TextStyle(
    color: AppColors.textSecondary,
    fontSize: 14,
  );
}