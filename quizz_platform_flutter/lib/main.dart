import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'presentation/pages/main_page.dart';
import 'presentation/pages/build_quiz_page.dart';
import 'styles/app_colors.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const ProviderScope(child: QuizApp()));
}

class QuizApp extends StatelessWidget {
  const QuizApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Quiz Platform',
      debugShowCheckedModeBanner: false,
      
      theme: ThemeData(
        useMaterial3: true,
        brightness: Brightness.dark, 
        
        colorScheme: ColorScheme.dark(
          primary: AppColors.accent, // Akcenty (np. kursory, zaznaczenia) na żółto
          secondary: Colors.greenAccent, // Przyciski akcji
          surface: AppColors.primary, // Tło kart i dialogów
          onPrimary: AppColors.primary, // Tekst na elementach "primary"
          onSurface: Colors.white, // Tekst na kartach
        ),

        scaffoldBackgroundColor: AppColors.primaryLight,
        
        // Dopasowanie pól tekstowych (TextField) w całej aplikacji
        inputDecorationTheme: InputDecorationTheme(
          filled: true,
          fillColor: Colors.white.withValues(alpha: 0.05),
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: BorderSide(color: Colors.white.withValues(alpha: 0.2)),
          ),
          enabledBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: BorderSide(color: Colors.white.withValues(alpha: 0.1)),
          ),
          focusedBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: const BorderSide(color: AppColors.accent),
          ),
          labelStyle: const TextStyle(color: AppColors.accent),
          hintStyle: const TextStyle(color: Colors.white30),
        ),

        textTheme: const TextTheme(
          displayLarge: TextStyle(color: AppColors.accent, fontWeight: FontWeight.bold),
          bodyLarge: TextStyle(color: Colors.white, fontSize: 16),
          bodyMedium: TextStyle(color: Colors.white70),
        ),

        // Stylizacja przycisków
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: Colors.greenAccent,
            foregroundColor: AppColors.primary,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          ),
        ),
      ),

      initialRoute: '/',
      routes: {
        '/': (context) => const MainPage(),
        '/build': (context) => const BuildQuizPage(),
      },
    );
  }
}