import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:quizz_platform_flutter/data/models/question.dart';
import 'package:quizz_platform_flutter/data/models/quiz.dart';

import '../models/quiz.dart';

class ApiHandler {
  final String baseUri = "https://localhost:7146/api";

  // --- QUIZZES ---

  Future<List<Quiz>> getQuizData() async {
    final uri = Uri.parse("$baseUri/quizzes");
    try {
      final response = await http.get(
        uri,
        headers: {'Content-type': 'application/json; charset=UTF-8'},
      );

      if (response.statusCode >= 200 && response.statusCode <= 299) {
        final List<dynamic> jsonData = json.decode(response.body);
        return jsonData.map((json) => Quiz.fromJson(json)).toList();
      }
    } catch (_) {}
    return [];
  }

  Future<int?> postQuiz(Quiz quiz) async {
    final url = Uri.parse("$baseUri/admin/quizzes");
    try {
      final response = await http.post(
        url,
        headers: {"Content-Type": "application/json"},
        body: jsonEncode(quiz.toJson()),
      );

      if (response.statusCode == 201) {
        final data = jsonDecode(response.body);
        return data['quiz']['id'];
      }
    } catch (_) {}
    return null;
  }

  Future<bool> updateQuiz(Quiz quiz) async {
    final url = Uri.parse("$baseUri/admin/quizzes/${quiz.id}");
    try {
      final response = await http.put(
        url,
        headers: {"Content-Type": "application/json"},
        body: jsonEncode(quiz.toJson()),
      );
      return response.statusCode == 200;
    } catch (_) {
      return false;
    }
  }

  Future<bool> deleteQuiz(int id) async {
    final url = Uri.parse("$baseUri/admin/quizzes/$id");
    try {
      final response = await http.delete(url);
      return response.statusCode == 204 || response.statusCode == 200;
    } catch (_) {
      return false;
    }
  }

  Future<Quiz?> getFullQuiz(int id) async {
    final url = Uri.parse("$baseUri/admin/quizzes/$id");
    try {
      final response = await http.get(url);
      if (response.statusCode == 200) {
        return Quiz.fromJson(jsonDecode(response.body));
      }
    } catch (_) {}
    return null;
  }

  // --- QUESTIONS ---

  Future<List<Question>> getQuestionsForQuiz(int quizId) async {
    final url = Uri.parse("$baseUri/admin/quizzes/$quizId/questions");
    try {
      final response = await http.get(url);
      if (response.statusCode == 200) {
        final List<dynamic> data = jsonDecode(response.body);
        return data.map((q) => Question.fromJson(q)).toList();
      }
    } catch (_) {}
    return [];
  }

  Future<bool> addQuestionToQuiz(int quizId, Question question) async {
    final url = Uri.parse("$baseUri/admin/quizzes/$quizId/questions");
    try {
      final response = await http.post(
        url,
        headers: {"Content-Type": "application/json"},
        body: jsonEncode(question.toJson()),
      );
      return response.statusCode == 201;
    } catch (_) {
      return false;
    }
  }

  Future<bool> updateQuestion(Question question) async {
    if (question.id == null) return false;
    final url = Uri.parse("$baseUri/admin/questions/${question.id}");
    try {
      final response = await http.put(
        url,
        headers: {"Content-Type": "application/json"},
        body: jsonEncode(question.toJson()),
      );
      return response.statusCode == 200;
    } catch (_) {
      return false;
    }
  }

  Future<bool> deleteQuestion(int questionId) async {
    final url = Uri.parse("$baseUri/admin/questions/$questionId");
    try {
      final response = await http.delete(url);
      return response.statusCode == 200;
    } catch (_) {
      return false;
    }
  }
}